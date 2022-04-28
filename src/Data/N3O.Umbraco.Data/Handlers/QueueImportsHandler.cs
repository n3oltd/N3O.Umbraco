using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Data.Konstrukt;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Data.Services;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.References;
using N3O.Umbraco.Storage.Extensions;
using N3O.Umbraco.Storage.Services;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Extensions;

namespace N3O.Umbraco.Data.Handlers {
    public class QueueImportsHandler : IRequestHandler<QueueImportsCommand, QueueImportsReq, QueueImportsRes> {
        private const string CountersKey = "Import";
        private const int MaxRowsCount = 5_000;

        private readonly List<string> _errors = new();
        private readonly ICounters _counters;
        private readonly IJsonProvider _jsonProvider;
        private readonly IContentService _contentService;
        private readonly IReadOnlyList<IImportPropertyFilter> _propertyFilters;
        private readonly IReadOnlyList<IPropertyConverter> _converters;
        private readonly IContentTypeService _contentTypeService;
        private readonly IDataTypeService _dataTypeService;
        private readonly ILocalClock _clock;
        private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;
        private readonly IWorkspace _workspace;
        private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
        private readonly IImportProcessingQueue _importProcessingQueue;
        private readonly Lazy<IVolume> _volume;
        private readonly ITempStorage _tempStorage;
        private readonly IFormatter _formatter;
        private IReadOnlyDictionary<string, Guid> _existingReferences;

        public QueueImportsHandler(IBackOfficeSecurityAccessor backOfficeSecurityAccessor,
                                   IWorkspace workspace,
                                   ILocalClock clock,
                                   ICounters counters,
                                   IJsonProvider jsonProvider,
                                   IEnumerable<IPropertyConverter> converters,
                                   IEnumerable<IImportPropertyFilter> propertyFilters,
                                   IContentService contentService,
                                   IContentTypeService contentTypeService,
                                   IDataTypeService dataTypeService,
                                   IUmbracoDatabaseFactory umbracoDatabaseFactory,
                                   IImportProcessingQueue importProcessingQueue,
                                   Lazy<IVolume> volume,
                                   ITempStorage tempStorage,
                                   IFormatter formatter) {
            _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
            _workspace = workspace;
            _clock = clock;
            _counters = counters;
            _jsonProvider = jsonProvider;
            _contentService = contentService;
            _converters = converters.ToList();
            _propertyFilters = propertyFilters.ToList();
            _contentTypeService = contentTypeService;
            _dataTypeService = dataTypeService;
            _umbracoDatabaseFactory = umbracoDatabaseFactory;
            _importProcessingQueue = importProcessingQueue;
            _volume = volume;
            _tempStorage = tempStorage;
            _formatter = formatter;
        }
        
        public async Task<QueueImportsRes> Handle(QueueImportsCommand req, CancellationToken cancellationToken) {
            try {
                var count = await QueueAsync(req, cancellationToken);

                return new QueueImportsRes {
                    Success = true, Count = count
                };
            } catch (HasErrorsException) {
                return new QueueImportsRes {
                    Success = false,
                    Errors = _errors
                };
            } catch (Exception ex) {
                return new QueueImportsRes {
                    Success = false,
                    Errors = ex.ToString().Yield()
                };
            }
        }
        
        private async Task<int> QueueAsync(QueueImportsCommand req, CancellationToken cancellationToken) {
            var csvBlob = await _tempStorage.GetFileAsync(req.Model.CsvFile.Filename, cancellationToken);

            try {
                using (csvBlob.Stream) {
                    var containerContent = req.ContentId.Run(_contentService.GetById, true);
                    var contentType = _contentTypeService.GetContentTypeForContainerContent(containerContent.ContentTypeId);

                    var propertyColumns = contentType.GetUmbracoProperties(_dataTypeService)
                                                     .Where(x => x.CanInclude(_propertyFilters))
                                                     .ToDictionary(x => x,
                                                                   x => _workspace.ColumnRangeBuilder
                                                                                  .GetColumns(x.GetTemplateColumn(_converters)));

                    var csvReader = _workspace.GetCsvReader(req.Model.DatePattern,
                                                            DecimalSeparators.Point,
                                                            BlobResolvers.Url(),
                                                            TextEncodings.Utf8,
                                                            csvBlob.Stream,
                                                            true);

                    ValidateColumns(csvReader.GetColumnHeadings(), propertyColumns.SelectMany(x => x.Value));

                    var currentUser = _backOfficeSecurityAccessor.BackOfficeSecurity.CurrentUser;
                    var imports = new List<Import>();
                    var batchReference = (int) await _counters.NextAsync(CountersKey, 100_001, cancellationToken);
                    var batchFilename = csvBlob.Filename;
                    var queuedAt = _clock.GetLocalNow().ToDateTimeUnspecified();

                    var storageFolderName = req.Model.ZipFile.HasValue() ? $"Import{batchReference}" : null;

                    if (req.Model.ZipFile != null) {
                        await ExtractToStorageFolderAsync(req.Model.ZipFile, storageFolderName);
                    }

                    var rowNumber = 1;
                    while (csvReader.ReadRow()) {
                        var replacesReference = csvReader.Row.GetRawField("Replaces Reference");

                        var import = new Import();
                        import.Reference = $"{batchReference}-{rowNumber}";
                        import.QueuedAt = queuedAt;
                        import.QueuedByUser = currentUser.Key;
                        import.QueuedByName = currentUser.Name;
                        import.BatchReference = batchReference.ToString();
                        import.BatchFilename = batchFilename;
                        import.FileRowNumber = rowNumber;
                        import.StorageFolderName = storageFolderName;
                        import.ContentTypeAlias = contentType.Alias;
                        import.ParentId = containerContent.Key;
                        import.ContentTypeName = contentType.Name;

                        if (replacesReference.HasValue()) {
                            import.Action = ImportActions.Update;
                            import.ReplacesId = FindExistingId(containerContent.Key, replacesReference);
                        } else {
                            import.Action = ImportActions.Create;
                        }

                        import.Fields = _jsonProvider.SerializeObject(GetFields(csvReader, propertyColumns));
                        import.Status = ImportStatuses.Queued;

                        imports.Add(import);

                        rowNumber++;
                    }

                    ValidateImports(imports);
                    
                    await InsertAndQueueAsync(imports);

                    return imports.Count;
                }
            } finally {
                await _tempStorage.DeleteFileAsync(csvBlob.Filename);
            }
        }

        private void ValidateColumns(IReadOnlyList<string> csvHeadings, IEnumerable<Column> expectedColumns) {
            var expectedHeadings = expectedColumns.Select(x => x.Title).ToList();
            var missingHeadings = expectedHeadings.Except(csvHeadings, StringComparer.InvariantCultureIgnoreCase)
                                                  .ToList();

            if (missingHeadings.Any()) {
                foreach (var missingHeading in missingHeadings) {
                    AddError(s => s.MissingColumn_1, missingHeading);
                }

                throw new HasErrorsException();
            }
        }
        
        private void ValidateImports(IReadOnlyList<Import> imports) {
            if (imports.Count > MaxRowsCount) {
                AddError(s => s.MaxRowsExceeded_1, MaxRowsCount);
            }
            
            if (_errors.Any()) {
                throw new HasErrorsException();
            }
        }

        private async Task ExtractToStorageFolderAsync(StorageToken zipStorageToken, string storageFolderName) {
            var zipBlob = await _tempStorage.GetFileAsync(zipStorageToken.Filename);

            try {
                using (zipBlob.Stream) {
                    var storageFolder = await _volume.Value.GetStorageFolderAsync(storageFolderName);

                    var zipArchive = new ZipArchive(zipBlob.Stream, ZipArchiveMode.Read);
                    await zipArchive.ExtractToStorageFolderAsync(storageFolder);
                }
            } finally {
                await _tempStorage.DeleteFileAsync(zipBlob.Filename);
            }
        }

        private Guid? FindExistingId(Guid containerId, string reference) {
            if (_existingReferences == null) {
                PopulateExistingReferences(containerId);
            }

            if (_existingReferences.ContainsKey(reference)) {
                return _existingReferences[reference];
            } else {
                AddError(s => s.ExistingRecordNotFound_1, reference);

                return null;
            }
        }

        private void PopulateExistingReferences(Guid containerId) {
            var dict = new Dictionary<string, Guid>(StringComparer.InvariantCultureIgnoreCase);

            _existingReferences = dict;
        }
        
        private IEnumerable<Field> GetFields(ICsvReader csvReader,
                                             IReadOnlyDictionary<UmbracoPropertyInfo, IEnumerable<Column>> propertyColumns) {
            foreach (var (property, columns) in propertyColumns) {
                foreach (var column in columns) {
                    var field = new Field();
                    field.Property = property.Type.Alias;
                    field.Name = column.Title;
                    field.Value = csvReader.Row.GetRawField(column.Title);

                    yield return field;
                }
            }
        }

        private async Task InsertAndQueueAsync(IReadOnlyList<Import> imports) {
            if (imports.Any()) {
                using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                    await db.InsertBatchAsync(imports);
                }

                _importProcessingQueue.AddAll(imports);
            }
        }
        
        private void AddError(Func<Strings, string> propertySelector, params object[] formatArgs) {
            _errors.Add(_formatter.Text.Format(propertySelector, formatArgs));
        }

        public class Strings : CodeStrings {
            public string ExistingRecordNotFound_1 => $"No existing record found with ID {"{0}".Quote()}";
            public string MaxRowsExceeded_1 => $"The CSV file contains more than the maximum allowed {0} rows";
            public string MissingColumn_1 => $"CSV file is missing column {"{0}".Quote()}";
        }

        private class HasErrorsException : Exception { }
    }
}