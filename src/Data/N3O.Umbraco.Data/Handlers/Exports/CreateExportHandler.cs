using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
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
        private readonly IReadOnlyList<IExportPropertyFilter> _propertyFilters;
        private readonly IReadOnlyList<IPropertyConverter> _propertyConverters;
        private readonly IReadOnlyList<IContentMetadataConverter> _metadataConverters;
        private readonly Lazy<IExcelTableBuilder> _excelTableBuilder;

        public CreateExportHandler(IContentService contentService,
                                   IContentTypeService contentTypeService,
                                   IDataTypeService dataTypeService,
                                   IContentHelper contentHelper,
                                   IWorkspace workspace,
                                   IEnumerable<IExportPropertyFilter> propertyFilters,
                                   IEnumerable<IPropertyConverter> propertyConverters,
                                   IEnumerable<IContentMetadataConverter> metadataConverters,
                                   Lazy<IExcelTableBuilder> excelTableBuilder) {
            _contentService = contentService;
            _contentTypeService = contentTypeService;
            _dataTypeService = dataTypeService;
            _contentHelper = contentHelper;
            _workspace = workspace;
            _propertyFilters = propertyFilters.ToList();
            _propertyConverters = propertyConverters.ToList();
            _metadataConverters = metadataConverters.ToList();
            _excelTableBuilder = excelTableBuilder;
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

            var metadataConverters = GetMetadataConverters(req.Model.Metadata);

            var publishedOnly = !req.Model.IncludeUnpublished.GetValueOrThrow();

            for (var pageIndex = 0; true; pageIndex++) {
                var page = _contentService.GetPagedDescendants(containerContent.Id,
                                                               pageIndex,
                                                               PageSize,
                                                               out var totalRecords);

                foreach (var content in page) {
                    if (content.ContentType.Id != contentType.Id) {
                        continue;
                    }
                    
                    if (publishedOnly && !content.Published) {
                        continue;
                    }

                    foreach (var (columnRange, converter) in metadataConverters) {
                        tableBuilder.AddValue(columnRange, converter.GetValue(content));    
                    }

                    var contentProperties = _contentHelper.GetContentProperties(content);

                    var columnOrder = 1000;
                    foreach (var propertyInfo in propertyInfos) {
                        var converter = propertyInfo.GetPropertyConverter(_propertyConverters);
                        var contentProperty = contentProperties.GetPropertyByAlias(propertyInfo.Type.Alias);

                        converter.Export(tableBuilder,
                                         _propertyConverters,
                                         columnOrder,
                                         null,
                                         contentProperty,
                                         propertyInfo);

                        columnOrder += 1000;
                    }
                    
                    tableBuilder.NextRow();
                }

                if (totalRecords < (pageIndex + 1) * PageSize) {
                    break;
                }
            }
            
            var table = tableBuilder.Build();

            WorkbookFormat workbookFormat;
            byte[] contents;

            using (var stream = new MemoryStream()) {
                if (req.Model.Format == WorkbookFormats.Csv) {
                    workbookFormat = WorkbookFormats.Csv;
                    await WriteCsvAsync(table, stream);
                } else if (req.Model.Format == WorkbookFormats.Excel) {
                    workbookFormat = WorkbookFormats.Excel;
                    await WriteExcelAsync(table, stream);
                } else {
                    throw UnrecognisedValueException.For(req.Model.Format);
                }

                stream.Rewind();
                contents = stream.ToArray();
            }

            return new ExportFile(workbookFormat.AppendFileExtension($"{contentType.Name} Export"),
                                  workbookFormat.ContentType,
                                  contents);
        }

        private IReadOnlyDictionary<IColumnRange, IContentMetadataConverter> GetMetadataConverters(IEnumerable<ContentMetadata> contentMetadatas) {
            var dict = new Dictionary<IColumnRange, IContentMetadataConverter>();

            foreach (var (contentMetadata, index) in contentMetadatas.SelectWithIndex()) {
                var converter = _metadataConverters.Single(x => x.IsConverter(contentMetadata));

                dict[converter.GetColumnRange(index)] = converter;
            }
            
            return dict;
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