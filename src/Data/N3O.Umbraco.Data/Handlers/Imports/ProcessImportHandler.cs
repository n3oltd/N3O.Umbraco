using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Exceptions;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Data.Konstrukt;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Storage;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Extensions;

namespace N3O.Umbraco.Data.Handlers;

public class ProcessImportHandler : IRequestHandler<ProcessImportCommand, None, None> {
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
    private readonly IContentEditor _contentEditor;
    private readonly IContentService _contentService;
    private readonly IContentTypeService _contentTypeService;
    private readonly IJsonProvider _jsonProvider;
    private readonly IClock _clock;
    private readonly IDataTypeService _dataTypeService;
    private readonly IParserFactory _parserFactory;
    private readonly IVolume _volume;
    private readonly IFormatter _formatter;
    private readonly ErrorLog _errorLog;
    private readonly IReadOnlyList<IPropertyConverter> _converters;
    private readonly IReadOnlyList<IImportPropertyFilter> _importPropertyFilters;
    private readonly IReadOnlyList<IContentSummariser> _contentSummarisers;

    public ProcessImportHandler(IUmbracoDatabaseFactory umbracoDatabaseFactory,
                                IContentEditor contentEditor,
                                IContentService contentService,
                                IContentTypeService contentTypeService,
                                IJsonProvider jsonProvider,
                                IClock clock,
                                IDataTypeService dataTypeService,
                                IParserFactory parserFactory,
                                IVolume volume,
                                IFormatter formatter,
                                IEnumerable<IPropertyConverter> converters,
                                IEnumerable<IImportPropertyFilter> importPropertyFilters,
                                IEnumerable<IContentSummariser> contentSummarisers) {
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
        _contentEditor = contentEditor;
        _contentService = contentService;
        _contentTypeService = contentTypeService;
        _jsonProvider = jsonProvider;
        _clock = clock;
        _dataTypeService = dataTypeService;
        _parserFactory = parserFactory;
        _volume = volume;
        _formatter = formatter;
        _errorLog = new ErrorLog(formatter);
        _converters = converters.OrEmpty().ToList();
        _importPropertyFilters = importPropertyFilters.OrEmpty().ToList();
        _contentSummarisers = contentSummarisers.OrEmpty().ToList();
    }

    public async Task<None> Handle(ProcessImportCommand req, CancellationToken cancellationToken) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var import = await req.ImportId.RunAsync((id, _) => db.SingleByIdAsync<Import>(id),
                                                     true,
                                                     cancellationToken);

            if (import.CanProcess) {
                try {
                    var propertyInfos = GetPropertyInfos(import.ContentTypeAlias);
                    var parser = await GetParserAsync(import);
                    var propertyInfoFields = _jsonProvider.DeserializeObject<ImportData>(import.Data)
                                                          .Fields
                                                          .GroupBy(x => x.Property)
                                                          .Where(x => x.Any(f => f.Value.HasValue()))
                                                          .ToDictionary(x => propertyInfos[x.Key],
                                                                        x => x.ToList());
                    var importData = _jsonProvider.DeserializeObject<ImportData>(import.Data);
                    var contentPublisher = GetContentPublisher(import, importData.ContentId);
                    
                    foreach (var (propertyInfo, fields) in propertyInfoFields) {
                        ImportProperty(contentPublisher, parser, propertyInfo, fields);
                    }
                    
                    _errorLog.ThrowIfHasErrors();

                    SaveOrPublishContent(contentPublisher, import);
                } catch (ProcessingException processingException) {
                    import.Error(_jsonProvider, processingException.Errors);
                } catch (Exception ex) {
                    import.Error(_jsonProvider, ex);
                }

                await db.UpdateAsync(import);
            }
        }

        return None.Empty;
    }

    private IReadOnlyDictionary<string, UmbracoPropertyInfo> GetPropertyInfos(string importContentTypeAlias) {
        var contentType = _contentTypeService.Get(importContentTypeAlias);

        return contentType.GetUmbracoProperties(_dataTypeService, _contentTypeService)
                          .Where(x => x.CanInclude(_importPropertyFilters))
                          .ToDictionary(x => x.Type.Alias, x => x);
    }

    private async Task<IParser> GetParserAsync(Import import) {
        var parserSettings = _jsonProvider.DeserializeObject<ParserSettings>(import.ParserSettings);

        var resolvers = new List<IBlobResolver>();
        resolvers.Add(new UrlBlobResolver());

        if (parserSettings.StorageFolderName.HasValue()) {
            var storageFolder = await _volume.GetStorageFolderAsync(parserSettings.StorageFolderName);

            resolvers.Add(new StorageFolderBlobResolver(storageFolder));
        }

        var parser = _parserFactory.GetParser(parserSettings.DatePattern,
                                              parserSettings.DecimalSeparator,
                                              resolvers);

        return parser;
    }

    private IContentPublisher GetContentPublisher(Import import, Guid? contentId) {
        IContentPublisher contentPublisher;
        
        if (import.Action == ImportActions.Create) {
            var contentType = _contentTypeService.Get(import.ContentTypeAlias);

            contentPublisher = _contentEditor.New(import.Name, import.ContainerId, contentType.Alias, contentId);
        } else if (import.Action == ImportActions.Update) {
            contentPublisher = _contentEditor.ForExisting(import.ReplacesId.Value);

            if (import.MoveUpdatedContentToContainer) {
                contentPublisher.Move(import.ContainerId);
            }
        } else {
            throw UnrecognisedValueException.For(import.Action);
        }

        contentPublisher.Content.OnBuilt += (_, _) => _errorLog.ThrowIfHasErrors();

        return contentPublisher;
    }

    private string GetContentSummary(IContent content) {
        var summariser = _contentSummarisers.SingleOrDefault(x => x.IsSummariser(content.ContentType.Alias));
        
        return summariser?.GetSummary(content) ?? content.Name;
    }
    
    private IEnumerable<string> GetSaveWarnings(PublishResult publishResult) {
        foreach (var eventMessage in publishResult.EventMessages.GetAll()) {
            yield return eventMessage.Message;
        }
        
        foreach (var invalidProperty in publishResult.InvalidProperties.OrEmpty()) {
            yield return _formatter.Text.Format<Strings>(s => s.InvalidProperty_1,
                                                         invalidProperty.PropertyType.Name);
        }
    }
    
    private void ImportProperty(IContentPublisher contentPublisher,
                                IParser parser,
                                UmbracoPropertyInfo propertyInfo,
                                IReadOnlyList<ImportField> fields) {
        var converter = propertyInfo.GetPropertyConverter(_converters);

        converter.Import(contentPublisher.Content,
                         _converters,
                         parser,
                         _errorLog,
                         null,
                         propertyInfo,
                         fields);
    }
    
    private void PublishContent(IContentPublisher contentPublisher, Import import) {
        var publishResult = contentPublisher.SaveAndPublish();

        var savedContent = _contentService.GetById(publishResult.Content.Id);
        var wasSaved = savedContent != null;
        var wasPublished = publishResult.Success;

        if (wasSaved) {
            var contentSummary = GetContentSummary(savedContent);
                        
            if (wasPublished) {
                import.SavedAndPublished(_clock, savedContent.Key, contentSummary);
            } else {
                import.Saved(_jsonProvider,
                             _clock,
                             savedContent.Key,
                             contentSummary,
                             GetSaveWarnings(publishResult));
            }
        } else {
            import.Error(_jsonProvider, publishResult.EventMessages.GetAll().Select(x => x.Message));
        }
    }

    private void SaveContent(IContentPublisher contentPublisher, Import import) {
        contentPublisher.SaveUnpublished();

        var savedContent = _contentService.GetById(import.ReplacesId.Value);
                        
        var contentSummary = GetContentSummary(savedContent);

        var warningText = _formatter.Text.Format<Strings>(s => s.WasEdited_1, contentSummary);

        import.Saved(_jsonProvider,
                     _clock,
                     savedContent.Key,
                     contentSummary,
                     warningText.Yield());
    }
    
    private void SaveOrPublishContent(IContentPublisher contentPublisher, Import import) {
        if (import.MoveUpdatedContentToContainer && import.ReplacesId.HasValue) {
            var content = _contentService.GetById(import.ReplacesId.Value);

            if (!content.Edited && content.Published) {
                PublishContent(contentPublisher, import);
            } else {
                SaveContent(contentPublisher, import);
            }
        } else {
            PublishContent(contentPublisher, import);
        }
    }

    public class Strings : CodeStrings {
        public string InvalidProperty_1 => $"The {"{0}".Quote()} property is invalid";
        public string WasEdited_1 => $"Cannot publish edited content {"{0}".Quote()}";
    }
}
