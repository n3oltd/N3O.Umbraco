using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Entities;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Handlers;

public class ProcessExportHandler : IRequestHandler<ProcessExportCommand, ExportReq, None> {
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
    private readonly IRepository<Export> _repository;
    private readonly IVolume _volume;
    private readonly ICoreScopeProvider _coreScopeProvider;

    public ProcessExportHandler(IContentService contentService,
                                IContentTypeService contentTypeService,
                                IDataTypeService dataTypeService,
                                IContentHelper contentHelper,
                                IWorkspace workspace,
                                IEnumerable<IExportPropertyFilter> propertyFilters,
                                IEnumerable<IPropertyConverter> propertyConverters,
                                IEnumerable<IContentMetadataConverter> metadataConverters,
                                Lazy<IExcelTableBuilder> excelTableBuilder,
                                IRepository<Export> repository,
                                IVolume volume,
                                ICoreScopeProvider coreScopeProvider) {
        _contentService = contentService;
        _contentTypeService = contentTypeService;
        _dataTypeService = dataTypeService;
        _contentHelper = contentHelper;
        _workspace = workspace;
        _propertyFilters = propertyFilters.ToList();
        _propertyConverters = propertyConverters.ToList();
        _metadataConverters = metadataConverters.ToList();
        _excelTableBuilder = excelTableBuilder;
        _repository = repository;
        _volume = volume;
        _coreScopeProvider = coreScopeProvider;
    }

    public async Task<None> Handle(ProcessExportCommand req, CancellationToken cancellationToken) {
        var export = await req.ExportId.RunAsync(_repository.GetAsync, true, cancellationToken);
        var containerContent = _contentService.GetById(export.ContainerId);
        var contentType = _contentTypeService.Get(export.ContentType);

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

        var processedRecords = 0;
        for (var pageIndex = 0; true; pageIndex++) {
            var query = publishedOnly
                            ? _coreScopeProvider.CreateQuery<IContent>().Where(x => x.ContentTypeId == contentType.Id &
                                                                                    x.Published == true)
                            : _coreScopeProvider.CreateQuery<IContent>().Where(x => x.ContentTypeId == contentType.Id);

            var page = _contentService.GetPagedDescendants(containerContent.Id,
                                                           pageIndex,
                                                           PageSize,
                                                           out var totalRecords,
                                                           query);

            foreach (var content in page) {
                processedRecords++;

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

            export.Collated(processedRecords);

            await _repository.UpdateAsync(export);

            if (totalRecords < (pageIndex + 1) * PageSize) {
                break;
            }
        }

        export.Formatting();

        await _repository.UpdateAsync(export);
        
        var table = tableBuilder.Build();

        using (var stream = new MemoryStream()) {
            WorkbookFormat workbookFormat;

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

            var storageFolder = await _volume.GetStorageFolderAsync(export.StorageFolderName);
            await storageFolder.AddFileAsync(export.Filename, stream);

            export.Complete(StorageConstants.StorageFolders.Temp, export.Filename);

            await _repository.UpdateAsync(export);
        }

        return None.Empty;
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
        workbook.AddWorksheet().InsertTable(_excelTableBuilder.Value.ForTable(table).Build());
        workbook.FormatAsTable(true);
        
        await workbook.SaveAsync(stream);
    }
}
