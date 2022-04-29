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
using N3O.Umbraco.Storage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Data.Handlers {
    public class ProcessImport : IRequestHandler<ProcessImportCommand, None, None> {
        private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
        private readonly IContentEditor _contentEditor;
        private readonly IContentTypeService _contentTypeService;
        private readonly IJsonProvider _jsonProvider;
        private readonly IDataTypeService _dataTypeService;
        private readonly IParserFactory _parserFactory;
        private readonly IVolume _volume;
        private readonly ErrorLog _errorLog;
        private readonly IReadOnlyList<IPropertyConverter> _converters;
        private readonly IReadOnlyList<IImportPropertyFilter> _importPropertyFilters;
        private readonly IReadOnlyList<IContentSummariser> _contentSummarisers;

        public ProcessImport(IUmbracoDatabaseFactory umbracoDatabaseFactory,
                             IContentEditor contentEditor,
                             IContentTypeService contentTypeService,
                             IContentHelper contentHelper,
                             IJsonProvider jsonProvider,
                             IDataTypeService dataTypeService,
                             IParserFactory parserFactory,
                             IVolume volume,
                             IFormatter formatter,
                             IEnumerable<IPropertyConverter> converters,
                             IEnumerable<IImportPropertyFilter> importPropertyFilters,
                             IEnumerable<IContentSummariser> contentSummarisers) {
            _umbracoDatabaseFactory = umbracoDatabaseFactory;
            _contentEditor = contentEditor;
            _contentTypeService = contentTypeService;
            _jsonProvider = jsonProvider;
            _dataTypeService = dataTypeService;
            _parserFactory = parserFactory;
            _volume = volume;
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
                        var contentPublisher = GetContentPublisher(import);
                        var parser = await GetParserAsync(import);
                        var propertyInfoFields = _jsonProvider.DeserializeObject<IEnumerable<ImportField>>(import.Fields)
                                                              .GroupBy(x => x.Property)
                                                              .ToDictionary(x => propertyInfos[x.Key],
                                                                            x => x.ToList());

                        foreach (var (propertyInfo, fields) in propertyInfoFields) {
                            ImportProperty(contentPublisher, parser, propertyInfo, fields);
                        }
                        
                        _errorLog.ThrowIfHasErrors();

                        var publishResult = contentPublisher.SaveAndPublish();

                        var contentSummary = GetContentSummary(publishResult.Content);

                        if (publishResult.Success) {
                            import.Published(publishResult.Content.Key, contentSummary);
                        } else {
                            import.PublishingFailed(publishResult.Content.Key, contentSummary);
                        }
                    } catch (ProcessingException processingException) {
                        import.Error(processingException.Errors);
                    } catch (Exception ex) {
                        import.Error(ex);
                    }

                    await db.UpdateAsync(import);
                }
            }

            return None.Empty;
        }

        private IReadOnlyDictionary<string, UmbracoPropertyInfo> GetPropertyInfos(string importContentTypeAlias) {
            var contentType = _contentTypeService.Get(importContentTypeAlias);

            return contentType.GetUmbracoProperties(_dataTypeService)
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

        private IContentPublisher GetContentPublisher(Import import) {
            IContentPublisher contentPublisher;
            
            if (import.Action == ImportActions.Create) {
                var contentType = _contentTypeService.Get(import.ContentTypeAlias);

                contentPublisher = _contentEditor.New("New", import.ParentId, contentType.Alias);
            } else if (import.Action == ImportActions.Update) {
                contentPublisher = _contentEditor.ForExisting(import.ReplacesId.Value);
            } else {
                throw UnrecognisedValueException.For(import.Action);
            }

            contentPublisher.Content.OnBuilt += (_, _) => _errorLog.ThrowIfHasErrors();

            return contentPublisher;
        }

        private void ImportProperty(IContentPublisher contentPublisher,
                                    IParser parser,
                                    UmbracoPropertyInfo propertyInfo,
                                    IReadOnlyList<ImportField> fields) {
            var converter = _converters.Single(x => x.IsConverter(propertyInfo));

            converter.Import(contentPublisher.Content,
                             parser,
                             _errorLog,
                             propertyInfo,
                             fields.Where(x => !x.Ignore).ToList());
        }

        private string GetContentSummary(IContent content) {
            var summariser = _contentSummarisers.SingleOrDefault(x => x.IsSummariser(content.ContentType.Alias));
            
            return summariser?.GetSummary(content) ?? content.Name;
        }
    }
}