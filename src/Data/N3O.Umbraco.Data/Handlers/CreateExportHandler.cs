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
        private readonly IReadOnlyList<IPropertyConverter> _propertyConverters;
        private readonly IReadOnlyList<IExportPropertyFilter> _propertyFilters;

        public CreateExportHandler(IContentService contentService,
                                   IContentTypeService contentTypeService,
                                   IDataTypeService dataTypeService,
                                   IContentHelper contentHelper,
                                   IWorkspace workspace,
                                   IEnumerable<IExportPropertyFilter> propertyFilters,
                                   IEnumerable<IPropertyConverter> propertyConverters,
                                   Lazy<IExcelTableBuilder> excelTableBuilder) {
            _contentService = contentService;
            _contentTypeService = contentTypeService;
            _dataTypeService = dataTypeService;
            _contentHelper = contentHelper;
            _workspace = workspace;
            _excelTableBuilder = excelTableBuilder;
            _propertyConverters = propertyConverters.ToList();
            _propertyFilters = propertyFilters.ToList();
        }

        public async Task<ExportFile> Handle(CreateExportCommand req, CancellationToken cancellationToken) {
            var containerContent = req.ContentId.Run(_contentService.GetById, true);
            var contentType = _contentTypeService.GetContentTypeForContainerContent(containerContent.ContentTypeId);

            bool Filter(UmbracoPropertyInfo property) {
                return property.CanInclude(_propertyFilters) &&
                       req.Model.Properties.Contains(property.Type.Alias, StringComparer.InvariantCultureIgnoreCase);
            }
            
            var propertyInfos = contentType.GetUmbracoProperties(_dataTypeService).Where(Filter).ToList();
            
            var tableBuilder = _workspace.TableBuilder.Untyped(contentType.Name);

            var propertyColumnRanges = propertyInfos.Select(p => (p,
                                                                  _workspace.ColumnRangeBuilder
                                                                            .String<string>()
                                                                            .Title(p.GetName())
                                                                            .Build()))
                                                    .ToList();

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

                    var contentProperties = _contentHelper.GetContentProperties(content);

                    foreach (var (propertyInfo, columnRange) in propertyColumnRanges) {
                        var converter = _propertyConverters.Single(x => x.IsConverter(propertyInfo));

                        tableBuilder.AddCells(columnRange, converter.Export(contentProperties, propertyInfo));
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

                stream.Seek(0, SeekOrigin.Begin);
                contents = stream.ToArray();
            }

            return new ExportFile($"{contentType.Name} Export.{fileExtension}", mimeType, contents);
        }

        private async Task WriteCsvAsync(ITable table, Stream stream) {
            var workbook = _workspace.CreateCsvWorkbook();
            workbook.Headers(true);
            workbook.Encoding(TextEncodings.Utf8);
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