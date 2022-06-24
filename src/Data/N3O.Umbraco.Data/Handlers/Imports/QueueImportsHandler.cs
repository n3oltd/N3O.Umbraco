using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Data.Konstrukt;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Data.Providers;
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
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Data.Handlers;

public class QueueImportsHandler : IRequestHandler<QueueImportsCommand, QueueImportsReq, QueueImportsRes> {
    private const string CountersKey = "Import";
    private const int MaxRowsCount = 5_000;

    private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;
    private readonly IWorkspace _workspace;
    private readonly ILocalClock _clock;
    private readonly ICounters _counters;
    private readonly IJsonProvider _jsonProvider;
    private readonly IContentHelper _contentHelper;
    private readonly IContentService _contentService;
    private readonly IContentTypeService _contentTypeService;
    private readonly IDataTypeService _dataTypeService;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
    private readonly IImportProcessingQueue _importProcessingQueue;
    private readonly Lazy<IVolume> _volume;
    private readonly IEnumerable<IPropertyConverter> _propertyConverters;
    private readonly ErrorLog _errorLog;
    private readonly IReadOnlyList<IImportPropertyFilter> _propertyFilters;
    private readonly List<IContentMatcher> _contentMatchers;
    private readonly string _nameColumnTitle;
    private readonly string _replacesColumnTitle;
    private readonly string _contentIdColumnTitle;
    private IReadOnlyList<IContent> _descendants;

    public QueueImportsHandler(IBackOfficeSecurityAccessor backOfficeSecurityAccessor,
                               IWorkspace workspace,
                               ILocalClock clock,
                               ICounters counters,
                               IJsonProvider jsonProvider,
                               IContentHelper contentHelper,
                               IContentService contentService,
                               IContentTypeService contentTypeService,
                               IDataTypeService dataTypeService,
                               IUmbracoDatabaseFactory umbracoDatabaseFactory,
                               IImportProcessingQueue importProcessingQueue,
                               Lazy<IVolume> volume,
                               IFormatter formatter,
                               IEnumerable<IImportPropertyFilter> propertyFilters,
                               IEnumerable<IPropertyConverter> propertyConverters,
                               IEnumerable<IContentMatcher> contentMatchers) {
        _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
        _workspace = workspace;
        _clock = clock;
        _counters = counters;
        _jsonProvider = jsonProvider;
        _contentHelper = contentHelper;
        _contentService = contentService;
        _contentTypeService = contentTypeService;
        _dataTypeService = dataTypeService;
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
        _importProcessingQueue = importProcessingQueue;
        _volume = volume;
        _errorLog = new ErrorLog(formatter);
        _propertyConverters = propertyConverters.ToList();
        _propertyFilters = propertyFilters.ToList();
        _contentMatchers = contentMatchers.ToList();

        _nameColumnTitle = formatter.Text.Format<DataStrings>(s => s.NameColumnTitle);
        _replacesColumnTitle = formatter.Text.Format<DataStrings>(s => s.ReplacesColumnTitle);
        _contentIdColumnTitle = formatter.Text.Format<DataStrings>(s => s.ContentIdColumnTitle);
    }
    
    public async Task<QueueImportsRes> Handle(QueueImportsCommand req, CancellationToken cancellationToken) {
        var storageFolderName = _clock.GetCurrentInstant().ToUnixTimeMilliseconds().ToString();
        var csvBlob = await _volume.Value.MoveFileAsync(req.Model.CsvFile.Filename,
                                                        req.Model.CsvFile.StorageFolderName,
                                                        storageFolderName);
        
        using (csvBlob.Stream) {
            var containerContent = req.ContentId.Run(_contentService.GetById, true);
            var contentType = _contentTypeService.Get(req.ContentType);
            var propertyInfos = contentType.GetUmbracoProperties(_dataTypeService, _contentTypeService).ToList();
            var propertyInfoColumns = propertyInfos.Where(x => x.HasPropertyConverter(_propertyConverters) &&
                                                               x.CanInclude(_propertyFilters))
                                                   .ToDictionary(x => x,
                                                                 x => x.GetColumns(_propertyConverters));

            var csvReader = _workspace.GetCsvReader(req.Model.DatePattern,
                                                    DecimalSeparators.Point,
                                                    BlobResolvers.Url(),
                                                    TextEncodings.Utf8,
                                                    csvBlob.Stream,
                                                    true);

            ValidateColumns(csvReader.GetColumnHeadings(), propertyInfoColumns.SelectMany(x => x.Value));

            var currentUser = _backOfficeSecurityAccessor.BackOfficeSecurity.CurrentUser;
            var imports = new List<Import>();
            var batchReference = (int) await _counters.NextAsync(CountersKey, 100_001, cancellationToken);
            var batchFilename = csvBlob.Filename;
            var queuedAt = _clock.GetLocalNow().ToDateTimeUnspecified();
            var canReplace = csvReader.GetColumnHeadings().Contains(_replacesColumnTitle, true);
            var hasNameColumn = csvReader.GetColumnHeadings().Contains(_nameColumnTitle, true);
            var hasContentId = csvReader.GetColumnHeadings().Contains(_contentIdColumnTitle, true);
            
            var contentMatchers = _contentMatchers.Where(x => x.IsMatcher(contentType.Alias)).ToList();
            var parserSettings =  _jsonProvider.SerializeObject(new ParserSettings(req.Model.DatePattern,
                                                                                   DecimalSeparators.Point,
                                                                                   storageFolderName));

            if (req.Model.ZipFile != null) {
                await ExtractToStorageFolderAsync(req.Model.ZipFile, storageFolderName);
            }

            var rowNumber = 1;
            while (csvReader.ReadRow()) {
                var import = new Import();
                import.Reference = $"{batchReference}-{rowNumber}";
                import.Name = GetImportName(csvReader, hasNameColumn, import.Reference);
                import.QueuedAt = queuedAt;
                import.QueuedByUser = currentUser.Key;
                import.QueuedByName = currentUser.Name;
                import.BatchReference = batchReference.ToString();
                import.BatchFilename = batchFilename;
                import.FileRowNumber = rowNumber;
                import.ParserSettings = parserSettings;
                import.ContentTypeAlias = contentType.Alias;
                import.ParentId = containerContent.Key;
                import.ContentTypeName = contentType.Name;

                var replacesCriteria = canReplace
                                           ? csvReader.Row.GetRawField(_replacesColumnTitle)
                                           : null;
                var contentIdField = csvReader.Row.GetRawField(_contentIdColumnTitle);
                var contentId = hasContentId && contentIdField.HasValue()
                                    ? Guid.Parse(contentIdField)
                                    : default(Guid?);
                
                if (replacesCriteria.HasValue() && contentId.HasValue()) {
                    _errorLog.AddError<Strings>(s => s.CannotSpecifyContentIdAndReplaces_2,
                                                _contentIdColumnTitle,
                                                _replacesColumnTitle);
                }
                
                _errorLog.ThrowIfHasErrors();
                
                if (replacesCriteria.HasValue()) {
                    import.Action = ImportActions.Update;
                    import.ReplacesId = FindExistingId(containerContent,
                                                       contentType.Alias,
                                                       contentMatchers,
                                                       replacesCriteria);
                } else {
                    import.Action = ImportActions.Create;
                }

                var importData = new ImportData();
                importData.Reference = import.Reference;
                importData.Fields = GetFields(csvReader, propertyInfoColumns);
                importData.ContentId = contentId;

                import.Data = _jsonProvider.SerializeObject(importData);
                import.Status = ImportStatuses.Queued;

                imports.Add(import);

                rowNumber++;
            }

            ValidateImports(imports);

            await InsertAndQueueAsync(imports);

            return new QueueImportsRes {
               Count = imports.Count
           };
        }
    }

    private string GetImportName(ICsvReader csvReader, bool hasNameColumn, string importReference) {
        string importName = null;
        
        if (hasNameColumn) {
            importName = csvReader.Row.GetRawField(_nameColumnTitle);
        }

        if (!importName.HasValue()) {
            importName = importReference;
        }

        return importName;
    }

    private void ValidateColumns(IReadOnlyList<string> csvHeadings, IEnumerable<Column> expectedColumns) {
        var expectedHeadings = expectedColumns.Select(x => x.Title).ToList();
        var missingHeadings = expectedHeadings.Except(csvHeadings, StringComparer.InvariantCultureIgnoreCase)
                                              .ToList();

        if (missingHeadings.Any()) {
            foreach (var missingHeading in missingHeadings) {
                _errorLog.AddError<Strings>(s => s.MissingColumn_1, missingHeading);
            }
        }
        
        _errorLog.ThrowIfHasErrors();
    }
    
    private void ValidateImports(IReadOnlyList<Import> imports) {
        if (imports.Count > MaxRowsCount) {
            _errorLog.AddError<Strings>(s => s.MaxRowsExceeded_1, MaxRowsCount);
        }
        
        _errorLog.ThrowIfHasErrors();
    }

    private async Task ExtractToStorageFolderAsync(StorageToken zipStorageToken, string storageFolderName) {
        var tempStorage = await _volume.Value.GetStorageFolderAsync(zipStorageToken.StorageFolderName);
        var storageFolder = await _volume.Value.GetStorageFolderAsync(storageFolderName);
        var zipBlob = await tempStorage.GetFileAsync(zipStorageToken.Filename);

        try {
            using (zipBlob.Stream) {
                var zipArchive = new ZipArchive(zipBlob.Stream, ZipArchiveMode.Read);
                
                await zipArchive.ExtractToStorageFolderAsync(storageFolder);
            }
        } finally {
            await tempStorage.DeleteFileAsync(zipBlob.Filename);
        }
    }

    private Guid? FindExistingId(IContent container,
                                 string contentTypeAlias,
                                 IReadOnlyList<IContentMatcher> contentMatchers,
                                 string criteria) {
        if (_descendants == null) {
            PopulateDescendants(container, contentTypeAlias);
        }

        var matches = new List<IContent>();
        foreach (var descendant in _descendants) {
            foreach (var contentMatcher in contentMatchers) {
                if (contentMatcher.IsMatch(descendant, criteria)) {
                    matches.Add(descendant);
                }
            }
        }

        if (matches.IsSingle()) {
            return matches.Single().Key;
        } else {
            if (matches.None()) {
                _errorLog.AddError<Strings>(s => s.NoContentMatched_1, criteria);
            } else {
                _errorLog.AddError<Strings>(s => s.MultipleContentMatched_1, criteria);
            }

            return null;
        }
    }

    private void PopulateDescendants(IContent container, string contentTypeAlias) {
        _descendants = _contentHelper.GetDescendants(container)
                                     .Where(x => x.ContentType.Alias.EqualsInvariant(contentTypeAlias))
                                     .ToList();
    }
    
    private IEnumerable<ImportField> GetFields(ICsvReader csvReader,
                                               IReadOnlyDictionary<UmbracoPropertyInfo, IReadOnlyList<Column>> propertyColumns) {
        foreach (var (property, columns) in propertyColumns) {
            foreach (var column in columns) {
                var field = new ImportField();
                field.Property = property.Type.Alias;
                field.Name = column.Title;
                field.SourceValue = csvReader.Row.GetRawField(column.Title);
                field.Value = field.SourceValue;
                field.IsFile = UploadDataTypes.Contains(property.DataType.EditorAlias);

                yield return field;
            }
        }
    }

    private async Task InsertAndQueueAsync(IReadOnlyList<Import> imports) {
        if (imports.Any()) {
            using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                foreach (var import in imports) {
                    await db.InsertAsync(import);
                }
            }

            _importProcessingQueue.AddAll(imports);
        }
    }
    
    public class Strings : CodeStrings {
        public string CannotSpecifyContentIdAndReplaces_2 => $"{"{0}".Quote()} and {"{1}".Quote()} columns cannot both be specified";
        public string MaxRowsExceeded_1 => $"The CSV file contains more than the maximum allowed {0} rows";
        public string MissingColumn_1 => $"CSV file is missing column {"{0}".Quote()}";
        public string MultipleContentMatched_1 => $"More than one content found for {"{0}".Quote()}";
        public string NoContentMatched_1 => $"No content found for {"{0}".Quote()}";
        
    }
}
