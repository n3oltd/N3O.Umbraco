using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Konstrukt;
using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Data.Handlers {
    public class ProcessImport : IRequestHandler<ProcessImportCommand, None, None> {
        private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
        private readonly IContentEditor _contentEditor;
        private readonly IContentTypeService _contentTypeService;
        private readonly IContentService _contentService;
        private readonly IReadOnlyList<IPropertyConverter> _converters;
        private readonly IJsonProvider _jsonProvider;
        private readonly IReadOnlyList<IImportPropertyFilter> _importPropertyFilters;
        private readonly IDataTypeService _dataTypeService;
        private readonly IParserFactory _parserFactory;
        private readonly IColumnRangeBuilder _columnRangeBuilder;

        public ProcessImport(IUmbracoDatabaseFactory umbracoDatabaseFactory,
                             IContentEditor contentEditor,
                             IEnumerable<PropertyConverter> converters,
                             IContentTypeService contentTypeService,
                             IContentService contentService,
                             IJsonProvider jsonProvider,
                             IDataTypeService dataTypeService,
                             IEnumerable<IImportPropertyFilter> importPropertyFilters,
                             IParserFactory parserFactory,
                             IColumnRangeBuilder columnRangeBuilder) {
            _umbracoDatabaseFactory = umbracoDatabaseFactory;
            _contentEditor = contentEditor;
            _contentTypeService = contentTypeService;
            _contentService = contentService;
            _jsonProvider = jsonProvider;
            _dataTypeService = dataTypeService;
            _parserFactory = parserFactory;
            _columnRangeBuilder = columnRangeBuilder;
            _importPropertyFilters = importPropertyFilters.ToList();
            _converters = converters.ToList();
        }

        public async Task<None> Handle(ProcessImportCommand req, CancellationToken cancellationToken) {
            using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                var import = await req.ImportId.RunAsync((id, _) => db.SingleByIdAsync<Import>(id),
                                                         true,
                                                         cancellationToken);

                if (import.CanProcess) {
                    try {
                        var contentPublisher = GetContentPublisher(import);

                        // Add method import called GetFields() where pass a JSON converter and let it deserialize
                        // the fields. Each field should have the propertyAlias, propertyType, and also the values,
                        // (an array of strings) + label etc. The field should be able to construct and UmbracoPropertyInfo
                        //var converter = _converters.Single(x => x.IsConverter(umbracoPropertyInfo));
                        //converter.Import(contentPublisher, umbracoPropertyInfo, values);
                        var fields = GetFields(import.ContentTypeAlias, import.Fields);

                        // TODO store the date pattern in last request and use it here
                        var parser = _parserFactory.GetParser(DatePatterns.YearMonthDay,
                                                              DecimalSeparators.Point,
                                                              BlobResolvers.Url());

                        foreach (var (property, values) in fields) {
                            var converter = _converters.Single(x => x.IsConverter(property));

                            converter.Import(contentPublisher.Content,
                                             parser,
                                             property,
                                             values);
                        }

                        var publishResult = contentPublisher.SaveAndPublish();

                        // Add these methods to import to set its status and also to include the ID + reference was set to
                        if (publishResult.Success) {
                            //import.Published(id, reference);
                        } else {
                            //import.Saved(id, reference);
                        }
                    } catch {
                        // Catch specific exceptions that correspond to parsing errors, e.g. date is malformed
                        // This can be a generic message of the form, "Cannot convert value '[whatever text was' to
                        // type [dataType]'. This is enough for people to figure out what they need to do.
                    }

                    await db.UpdateAsync(import);
                }
            }

            return None.Empty;
        }

        private IReadOnlyList<(UmbracoPropertyInfo, IEnumerable<string>)> GetFields(string contentTypeAlias, string json) {
            var contentType = _contentTypeService.Get(contentTypeAlias);

            var properties = contentType.GetUmbracoProperties(_dataTypeService)
                                        .Where(x => x.CanInclude(_importPropertyFilters))
                                        .Select(x => x.GetTemplateColumn(_converters))
                                        .Select(x => (Property: x.PropertyInfo, Headings: _columnRangeBuilder.GetColumnHeadings(x)))
                                        .ToList();

            var values = _jsonProvider.DeserializeObject<Dictionary<string, string>>(json);

            var result = new List<(UmbracoPropertyInfo, IEnumerable<string>)>();

            foreach (var (property, headings) in properties) {
                var propertyValues = new List<string>();
                foreach (var heading in headings) {
                    var key = values.Keys.Single(x => x.EqualsInvariant(heading));
                    var value = values[key];
                    propertyValues.Add(value);
                }

                result.Add((property, propertyValues));
            }

            return result;
        }

        private IContentPublisher GetContentPublisher(Import import) {
            if (import.Action == ImportActions.Create) {
                var contentType = _contentTypeService.Get(import.ContentTypeAlias);

                var container = _contentTypeService.GetContainer(contentType.ParentId);

                return _contentEditor.New("New", import.ParentId, contentType.Alias);
            } else if (import.Action == ImportActions.Update) {
                return _contentEditor.ForExisting(import.ReplacesId.Value);
            } else {
                throw UnrecognisedValueException.For(import.Action);
            }
        }
    }
}