using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Data.Services;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Handlers {
    public class GetImportTemplateHandler : IRequestHandler<GetImportTemplateQuery, ImportTemplateReq, ImportTemplate> {
        private readonly IWorkspace _workspace;
        private readonly IReadOnlyList<IImportPropertyFilter> _propertyFilters;
        private readonly IReadOnlyList<IPropertyConverter> _propertyConverters;
        private readonly IContentTypeService _contentTypeService;
        private readonly IDataTypeService _dataTypeService;
        private readonly string _nameColumnTitle;
        private readonly string _replacesColumnTitle;
        private readonly string _contentIdColumnTitle;

        public GetImportTemplateHandler(IWorkspace workspace,
                                        IEnumerable<IImportPropertyFilter> propertyFilters,
                                        IEnumerable<IPropertyConverter> propertyConverters,
                                        IContentTypeService contentTypeService,
                                        IDataTypeService dataTypeService,
                                        IFormatter formatter) {
            _workspace = workspace;
            _propertyFilters = propertyFilters.ToList();
            _propertyConverters = propertyConverters.ToList();
            _contentTypeService = contentTypeService;
            _dataTypeService = dataTypeService;
            
            _nameColumnTitle = formatter.Text.Format<DataStrings>(s => s.NameColumnTitle);
            _replacesColumnTitle = formatter.Text.Format<DataStrings>(s => s.ReplacesColumnTitle);
            _contentIdColumnTitle = formatter.Text.Format<DataStrings>(s => s.ContentKey);
        }

        public async Task<ImportTemplate> Handle(GetImportTemplateQuery req, CancellationToken cancellationToken) {
            var contentType = _contentTypeService.Get(req.ContentType);

            var columns = new List<Column>();

            if (req.Model.ImportGuid.GetValueOrThrow()) {
                columns.Add(GetColumn(_contentIdColumnTitle));
            }
            
            columns.Add(GetColumn(_replacesColumnTitle));
            columns.Add(GetColumn(_nameColumnTitle));

            columns.AddRange(contentType.GetUmbracoProperties(_dataTypeService, _contentTypeService)
                                        .Where(x => x.HasPropertyConverter(_propertyConverters) &&
                                                    x.CanInclude(_propertyFilters))
                                        .SelectMany(x => x.GetColumns(_propertyConverters))
                                        .ToList());

            using (var stream = new MemoryStream()) {
                var workbook = _workspace.CreateCsvWorkbook();
                workbook.Headers(true);
                await workbook.WriteTemplateAsync(columns, stream, cancellationToken);

                stream.Rewind();
                
                return new ImportTemplate($"{contentType.Name} Import.csv", stream.ToArray());
            }
        }

        private Column GetColumn(string columnTitle) {
            var columnRange = _workspace.ColumnRangeBuilder
                                        .String<string>()
                                        .Title(columnTitle)
                                        .Build();
            
            columnRange.AddValues(0, null);

            return columnRange.GetColumns().Single();
        }
    }
}