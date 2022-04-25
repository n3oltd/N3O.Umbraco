using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Data.Konstrukt;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
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

namespace N3O.Umbraco.Data.Handlers {
    public class QueueImportsHandler : IRequestHandler<QueueImportsCommand, QueueImportsReq, None> {
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
                                   Lazy<IVolume> volume) {
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
        }

        // TODO Return type here should not be None, as we need to gracefully return a type to indicate 
        // in the case of failure what the issue was, e.g. should validate max 10k rows and fail if more,
        // should indicate malformed CSV file, should return error if existing reference could not be found,
        // and in the case of success should return how many records were imported.
        public async Task<None> Handle(QueueImportsCommand req, CancellationToken cancellationToken) {
            using (var stream = req.Model.CsvFile.OpenReadStream()) {
                var containerContent = req.ContentId.Run(_contentService.GetById, true);
                var contentType = _contentTypeService.GetContentTypeForContainerContent(containerContent.ContentTypeId);
                var properties = contentType.GetUmbracoProperties(_dataTypeService)
                                            .Where(x => x.CanInclude(_propertyFilters))
                                            .ToList();

                var templateColumns = properties.Select(x => x.GetTemplateColumn(_converters))
                                                .ToList();

                // TODO Add check here that CSV file has exactly template columns

                var csvReader = _workspace.GetCsvReader(req.Model.DatePattern,
                                                        DecimalSeparators.Point,
                                                        TextEncodings.Utf8,
                                                        stream,
                                                        true);

                var columnHeadings = csvReader.GetColumnHeadings();

                var currentUser = _backOfficeSecurityAccessor.BackOfficeSecurity.CurrentUser;
                var imports = new List<Import>();
                var batchReference = (int) await _counters.NextAsync("Import", 100_001, cancellationToken);
                var batchFilename = req.Model.CsvFile.Name;
                var queuedAt = _clock.GetLocalNow().ToDateTimeUnspecified();
                var storageFolderName = req.Model.ZipFile.HasValue() ? $"Import{batchReference}" : null;
                
                if (req.Model.ZipFile != null) {
                    await ExtractToStorageFolderAsync(req.Model.ZipFile, storageFolderName);
                }

                var rowNumber = 1;
                while (csvReader.ReadRow()) {
                    // TODO This needs to exclude "special" columns like the replaces reference
                    var rawValues = columnHeadings.ToDictionary(x => x, x => csvReader.Row.GetRawField(x));

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
                        import.ReplacesId = FindExistingId(replacesReference);
                    } else {
                        import.Action = ImportActions.Create;
                    }

                    import.Fields = _jsonProvider.SerializeObject(rawValues);
                    import.Status = ImportStatuses.Queued;

                    imports.Add(import);

                    rowNumber++;
                }

                await InsertAndQueueAsync(imports);

                return None.Empty;
            }
        }

        private async Task ExtractToStorageFolderAsync(IFormFile zipFile, string storageFolderName) {
            using (var stream = zipFile.OpenReadStream()) {
                var storageFolder = await _volume.Value.GetStorageFolderAsync(storageFolderName);

                var zipArchive = new ZipArchive(stream, ZipArchiveMode.Read);
                await zipArchive.ExtractToStorageFolderAsync(storageFolder);
            }
        }

        private Guid? FindExistingId(string replacesReference) {
            if (_existingReferences == null) {
                PopulateExistingReferences();
            }

            if (_existingReferences.ContainsKey(replacesReference)) {
                return _existingReferences[replacesReference];
            } else {
                // TODO See comment above in Handle method
                throw new Exception();
            }
        }

        private void PopulateExistingReferences() {
            var dict = new Dictionary<string, Guid>(StringComparer.InvariantCultureIgnoreCase);

            // TODO Take in as a parameter the parent ID and use IContent service to get all the existing child
            // content and map it into the dictionary

            _existingReferences = dict;
        }

        private async Task InsertAndQueueAsync(IReadOnlyList<Import> imports) {
            if (imports.Any()) {
                using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                    await db.InsertBatchAsync(imports);
                }

                _importProcessingQueue.AddAll(imports);
            }
        }
    }
}