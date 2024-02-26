using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Data.Konstrukt;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Providers;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using N3O.Umbraco.References;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Data;

public class ImportQueue : IImportQueue {
    private readonly List<Import> _imports = new();
    private readonly ICounters _counters;
    private readonly IJsonProvider _jsonProvider;
    private readonly IContentHelper _contentHelper;
    private readonly IContentService _contentService;
    private readonly IContentTypeService _contentTypeService;
    private readonly IDataTypeService _dataTypeService;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
    private readonly Lazy<IImportProcessingQueue> _importProcessingQueue;
    private readonly IEnumerable<IPropertyConverter> _propertyConverters;
    private readonly ErrorLog _errorLog;
    private readonly IReadOnlyList<IImportPropertyFilter> _propertyFilters;
    private readonly List<IContentMatcher> _contentMatchers;
    private IReadOnlyList<IContent> _descendants;
    private readonly ICoreScopeProvider _coreScopeProvider;

    public ImportQueue(ICounters counters,
                       IJsonProvider jsonProvider,
                       IContentHelper contentHelper,
                       IContentService contentService,
                       IContentTypeService contentTypeService,
                       IDataTypeService dataTypeService,
                       IUmbracoDatabaseFactory umbracoDatabaseFactory,
                       Lazy<IImportProcessingQueue> importProcessingQueue,
                       IFormatter formatter,
                       IEnumerable<IImportPropertyFilter> propertyFilters,
                       IEnumerable<IPropertyConverter> propertyConverters,
                       IEnumerable<IContentMatcher> contentMatchers,
                       ICoreScopeProvider coreScopeProvider) {
        _counters = counters;
        _jsonProvider = jsonProvider;
        _contentHelper = contentHelper;
        _contentService = contentService;
        _contentTypeService = contentTypeService;
        _dataTypeService = dataTypeService;
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
        _importProcessingQueue = importProcessingQueue;
        _coreScopeProvider = coreScopeProvider;
        _errorLog = new ErrorLog(formatter);
        _propertyConverters = propertyConverters.ToList();
        _propertyFilters = propertyFilters.ToList();
        _contentMatchers = contentMatchers.ToList();
    }

    public async Task AppendAsync(Guid containerId,
                                  string contentTypeAlias,
                                  string filename,
                                  int? row,
                                  Instant queuedAt,
                                  string queuedBy,
                                  DatePattern datePattern,
                                  string storageFolderName,
                                  Guid? contentId,
                                  string replacesCriteria,
                                  string name,
                                  bool moveUpdatedContentToContainer,
                                  IReadOnlyDictionary<string, string> sourceValues,
                                  CancellationToken cancellationToken = default) {
        var containerContent = _contentService.GetById(containerId);

        if (containerContent == null) {
            _errorLog.AddError<Strings>(s => s.ContainerNotFound_1, containerId);
        }
        
        var contentType = _contentTypeService.Get(contentTypeAlias);
        
        if (contentType == null) {
            _errorLog.AddError<Strings>(s => s.ContentTypeNotFound_1, contentTypeAlias);
        }
        
        _errorLog.ThrowIfHasErrors();
        
        var propertyInfos = contentType.GetUmbracoProperties(_dataTypeService, _contentTypeService).ToList();

        var propertyInfoColumns = propertyInfos.Where(x => x.HasPropertyConverter(_propertyConverters) &&
                                                           x.CanInclude(_propertyFilters))
                                               .ToDictionary(x => x, x => x.GetColumns(_propertyConverters));

        var import = new Import();
        import.Name = name ?? import.Reference;
        import.QueuedAt = queuedAt.ToDateTimeUtc();
        import.QueuedBy = queuedBy;
        import.Filename = filename;
        import.Row = row;
        import.ParserSettings = GetParserSettings(datePattern, storageFolderName);
        import.ContentTypeAlias = contentType.Alias;
        import.ContainerId = containerContent.Key;
        import.ContentTypeName = contentType.Name;
        import.MoveUpdatedContentToContainer = moveUpdatedContentToContainer;

        if (replacesCriteria.HasValue() && contentId.HasValue()) {
            _errorLog.AddError<Strings>(s => s.CannotSpecifyContentIdAndReplacementCriteria);
        }

        _errorLog.ThrowIfHasErrors();

        if (replacesCriteria.HasValue()) {
            import.Action = ImportActions.Update;
            import.ReplacesId = FindExistingId(containerContent, contentType, replacesCriteria);
        } else {
            import.Action = ImportActions.Create;
        }
        
        _errorLog.ThrowIfHasErrors();

        var reference = await _counters.NextAsync<ImportReferenceType>(cancellationToken);
        
        import.Reference = reference.ToString();
        import.Data = GetImportData(reference, contentId, sourceValues, propertyInfoColumns);
        import.Status = ImportStatuses.Queued;

        _imports.Add(import);
    }

    public async Task<int> CommitAsync(bool queueForProcessing = true) {
        if (_imports.Any()) {
            using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                foreach (var import in _imports) {
                    await db.InsertAsync(import);
                }
            }

            if (queueForProcessing) {
                _importProcessingQueue.Value.AddAll(_imports);
            }
        }

        var count = _imports.Count;
        
        _imports.Clear();

        return count;
    }
    
    private string GetParserSettings(DatePattern datePattern, string storageFolderName) {
        return _jsonProvider.SerializeObject(new ParserSettings(datePattern,
                                                                DecimalSeparators.Point,
                                                                storageFolderName));
    }
    
    private Guid? FindExistingId(IContent container,
                                 IContentType contentType,
                                 string criteria,
                                 HashSet<Guid> searched = null) {
        var contentMatchers = _contentMatchers.Where(x => x.IsMatcher(contentType.Alias)).ToList();
        
        if (_descendants == null) {
            PopulateDescendants(container, contentType.Id, searched);
        }

        var matches = new List<IContent>();
        
        foreach (var descendant in _descendants) {
            foreach (var contentMatcher in contentMatchers) {
                if (contentMatcher.IsMatch(descendant, criteria)) {
                    matches.Add(descendant);
                }
            }
        }

        if (matches.None() && container.ParentId != -1) {
            searched ??= new HashSet<Guid>();
            
            _descendants.OrEmpty().Select(x => x.Key).Do(x => searched.Add(x));
            
            _descendants = null;

            return FindExistingId(_contentService.GetById(container.ParentId), contentType, criteria, searched);
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

    private void PopulateDescendants(IContent container, int contentTypeId, HashSet<Guid> searched = null) {
        var query = _coreScopeProvider.CreateQuery<IContent>().Where(x => contentTypeId == x.ContentTypeId);
        
        if (searched.HasValue()) {
            query = query.Where(x => !searched.Contains(x.Key));
        }
        
        _descendants = _contentHelper.GetDescendants(container, query).ToList();
    }
    
    private string GetImportData(Reference reference,
                                 Guid? contentId,
                                 IReadOnlyDictionary<string, string> sourceValues,
                                 IReadOnlyDictionary<UmbracoPropertyInfo, IReadOnlyList<Column>> propertyInfoColumns) {
        var importData = new ImportData();
        importData.Reference = reference.ToString();
        importData.Fields = GetFields(sourceValues, propertyInfoColumns);
        importData.ContentId = contentId;

        return _jsonProvider.SerializeObject(importData);
    }

    private IEnumerable<ImportField> GetFields(IReadOnlyDictionary<string, string> sourceValues,
                                               IReadOnlyDictionary<UmbracoPropertyInfo, IReadOnlyList<Column>> propertyColumns) {
        foreach (var (property, columns) in propertyColumns) {
            foreach (var column in columns) {
                var field = new ImportField();
                field.Property = property.Type.Alias;
                field.Name = column.Title;
                field.SourceValue = sourceValues.GetValueOrDefault(column.Title);
                field.Value = field.SourceValue;
                field.IsFile = UploadDataTypes.Contains(property.DataType.EditorAlias);

                yield return field;
            }
        }
    }

    public class Strings : CodeStrings {
        public string CannotSpecifyContentIdAndReplacementCriteria => "Content ID and replacement criteria cannot both be specified";
        public string ContainerNotFound_1 => $"Container with ID {"{0}".Quote()} not found";
        public string ContentTypeNotFound_1 => $"Content type with alias {"{0}".Quote()} not found";
        public string MultipleContentMatched_1 => $"More than one content found for {"{0}".Quote()}";
        public string NoContentMatched_1 => $"No content found for {"{0}".Quote()}";
    }
}