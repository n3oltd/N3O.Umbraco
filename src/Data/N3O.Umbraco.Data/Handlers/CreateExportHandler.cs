using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Services;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Mediator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Handlers {
    public class CreateExportHandler : IRequestHandler<CreateExportCommand, ExportReq, ExportFile> {
        private const int PageSize = 100;
        
        private readonly IContentService _contentService;
        private readonly IContentTypeService _contentTypeService;
        private readonly IDataTypeService _dataTypeService;
        private readonly IContentHelper _contentHelper;
        private readonly IWorkspace _workspace;
        private readonly Lazy<IExcelTableBuilder> _excelTableBuilder;
        private readonly IReadOnlyList<IPropertyConverter> _converters;
        private readonly IReadOnlyList<IExportPropertyFilter> _propertyFilters;
        private readonly string _nameColumnTitle;

        public CreateExportHandler(IContentService contentService,
                                   IContentTypeService contentTypeService,
                                   IDataTypeService dataTypeService,
                                   IContentHelper contentHelper,
                                   IWorkspace workspace,
                                   IEnumerable<IExportPropertyFilter> propertyFilters,
                                   IEnumerable<IPropertyConverter> converters,
                                   Lazy<IExcelTableBuilder> excelTableBuilder,
                                   IFormatter formatter) {
            _contentService = contentService;
            _contentTypeService = contentTypeService;
            _dataTypeService = dataTypeService;
            _contentHelper = contentHelper;
            _workspace = workspace;
            _excelTableBuilder = excelTableBuilder;
            _converters = converters.ToList();
            _propertyFilters = propertyFilters.ToList();
            
            _nameColumnTitle = formatter.Text.Format<DataStrings>(s => s.NameColumnTitle);
        }

        public async Task<ExportFile> Handle(CreateExportCommand req, CancellationToken cancellationToken) {
            var containerContent = req.ContentId.Run(_contentService.GetById, true);
            var contentType = _contentTypeService.Get(req.ContentType);

            bool Filter(UmbracoPropertyInfo propertyInfo) {
                return propertyInfo.CanInclude(_propertyFilters) &&
                       req.Model.Properties.Contains(propertyInfo.Type.Alias,
                                                     StringComparer.InvariantCultureIgnoreCase);
            }
            
            var propertyInfos = contentType.GetUmbracoProperties(_dataTypeService, _contentTypeService)
                                           .Where(Filter)
                                           .ToList();
            
            var tableBuilder = _workspace.TableBuilder.Untyped(contentType.Name);

            var nameColumnRange = _workspace.ColumnRangeBuilder
                                            .String<string>()
                                            .Title(_nameColumnTitle)
                                            .Build();

            var publishedOnly = !req.Model.IncludeUnpublished.GetValueOrThrow();

            for (var pageIndex = 0; true; pageIndex++) {
                var page = _contentService.GetPagedChildren(containerContent.Id,
                                                            pageIndex,
                                                            PageSize,
                                                            out var totalRecords);

                foreach (var content in page) {
                    if (publishedOnly && !content.Published) {
                        continue;
                    }
                    
                    tableBuilder.AddValue(nameColumnRange, content.Name);

                    var contentProperties = _contentHelper.GetContentProperties(content);

                    foreach (var propertyInfo in propertyInfos) {
                        var converter = propertyInfo.GetPropertyConverter(_converters);
                        var contentProperty = contentProperties.GetPropertyByAlias(propertyInfo.Type.Alias);

                        converter.Export(tableBuilder, _converters, null, contentProperty, propertyInfo);
                    }
                    
                    tableBuilder.NextRow();
                }

                if (totalRecords < (pageIndex + 1) * PageSize) {
                    break;
                }
            }
            
            var table = tableBuilder.Build();

            string fileExtension;
            string mimeType;
            byte[] contents;

            using (var stream = new MemoryStream()) {
                if (req.Model.Format == WorkbookFormats.Csv) {
                    fileExtension = "csv";
                    mimeType = DataConstants.ContentTypes.Csv;
                    await WriteCsvAsync(table, stream);
                } else if (req.Model.Format == WorkbookFormats.Excel) {
                    fileExtension = "xlsx";
                    mimeType = DataConstants.ContentTypes.Excel;
                    await WriteExcelAsync(table, stream);
                } else {
                    throw UnrecognisedValueException.For(req.Model.Format);
                }

                stream.Rewind();
                contents = stream.ToArray();
            }

            return new ExportFile($"{contentType.Name} Export.{fileExtension}", mimeType, contents);
        }

        private async Task WriteCsvAsync(ITable table, Stream stream) {
            var workbook = _workspace.CreateCsvWorkbook();
            workbook.Headers(true);
            workbook.AddTable(table);

            await workbook.SaveAsync(stream);
        }

        private async Task WriteExcelAsync(ITable table, Stream stream) {
            var workbook = _workspace.CreateExcelWorkbook();
            workbook.AddWorksheet(_excelTableBuilder.Value.ForTable(table).Build());
            workbook.FormatAsTable(true);
            
            await workbook.SaveAsync(stream);
        }
    }
}