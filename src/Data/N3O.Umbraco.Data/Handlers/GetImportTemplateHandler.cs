using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Data.Services;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Handlers {
    public class GetImportTemplateHandler : IRequestHandler<GetImportTemplateQuery, None, ImportTemplate> {
        private readonly IWorkspace _workspace;
        private readonly IContentService _contentService;
        private readonly IReadOnlyList<IImportPropertyFilter> _propertyFilters;
        private readonly IReadOnlyList<IPropertyConverter> _converters;
        private readonly IContentTypeService _contentTypeService;
        private readonly IDataTypeService _dataTypeService;
        private readonly IColumnRangeBuilder _columnRangeBuilder;

        public GetImportTemplateHandler(IWorkspace workspace,
                                        IEnumerable<IImportPropertyFilter> propertyFilters,
                                        IEnumerable<IPropertyConverter> converters,
                                        IContentService contentService,
                                        IContentTypeService contentTypeService,
                                        IDataTypeService dataTypeService,
                                        IColumnRangeBuilder columnRangeBuilder) {
            _workspace = workspace;
            _contentService = contentService;
            _propertyFilters = propertyFilters.ToList();
            _converters = converters.ToList();
            _contentTypeService = contentTypeService;
            _dataTypeService = dataTypeService;
            _columnRangeBuilder = columnRangeBuilder;
        }

        public async Task<ImportTemplate> Handle(GetImportTemplateQuery req, CancellationToken cancellationToken) {
            var containerContent = req.ContentId.Run(_contentService.GetById, true);
            var contentType = _contentTypeService.GetContentTypeForContainerContent(containerContent.ContentTypeId);

            var columns = contentType.GetUmbracoProperties(_dataTypeService)
                                             .Where(x => x.CanInclude(_propertyFilters))
                                             .Select(x => x.GetTemplateColumn(_converters))
                                             .SelectMany(x => _columnRangeBuilder.GetColumns(x))
                                             .ToList();

            using (var stream = new MemoryStream()) {
                var workbook = _workspace.CreateCsvWorkbook();
                workbook.Encoding(TextEncodings.Utf8);
                workbook.Headers(true);
                await workbook.WriteTemplateAsync(columns, stream, cancellationToken);

                stream.Rewind();
                
                return new ImportTemplate($"{contentType.Name} Import.csv", stream.ToArray());
            }
        }
    }
}