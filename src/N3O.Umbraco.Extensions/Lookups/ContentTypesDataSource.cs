using N3O.Umbraco.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Api.Common.ViewModels.Pagination;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;
using Umbraco.Community.Contentment.DataEditors;
using Umbraco.Extensions;

namespace N3O.Umbraco.Lookups;

public class ContentTypesDataSource : IDataPickerSource, IDataSourceValueConverter {
    private static readonly ConcurrentDictionary<Guid, string> ContentTypeAliases = new();
    
    private readonly IContentTypeService _contentTypeService;
    
    public ContentTypesDataSource(IContentTypeService contentTypeService) {
        _contentTypeService = contentTypeService;
    }

    public string Name => "Umbraco Content Types";
    public string Icon => "icon-item-arrangement";
    public string Group => "N3O";
    public string Description => "A list of Umbraco content types";
    public Dictionary<string, object> DefaultValues => [];
    public IEnumerable<ContentmentConfigurationField> Fields => [];
    public OverlaySize OverlaySize => OverlaySize.Small;

    public IEnumerable<DataListItem> GetItems(Dictionary<string, object> config) {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DataListItem>> GetItemsAsync(Dictionary<string, object> config,
                                                         IEnumerable<string> values) {
        if (values.Any()) {
            var items = values.Select(x => UdiParser.TryParse(x, out GuidUdi udi) ? udi : null)
                              .WhereNotNull()
                              .Select(x => _contentTypeService.Get(x.Guid))
                              .WhereNotNull()
                              .Select(ToDataListItem);
            
            return Task.FromResult(items);
        }

        return Task.FromResult(Enumerable.Empty<DataListItem>());
    }

    public Task<PagedViewModel<DataListItem>> SearchAsync(Dictionary<string, object> config,
                                                          int pageNumber = 1,
                                                          int pageSize = 12,
                                                          string query = "") {
        var totalRecords = -1L;
        var pageIndex = pageNumber - 1;
        var items = _contentTypeService.GetAll();

        if (query.HasValue()) {
            items = items.Where(x => x.Name.InvariantContains(query) || x.Alias.InvariantContains(query));
        }

        if (items.Any()) {
            var offset = pageIndex * pageSize;
            
            var results = new PagedViewModel<DataListItem>();
            results.Total = totalRecords;
            
            results.Items = items.Skip(offset).Take(pageSize).Select(ToDataListItem);

            return Task.FromResult(results);
        }

        return Task.FromResult(new PagedViewModel<DataListItem>());
    }
    
    public Type GetValueType(Dictionary<string, object> config) => typeof(IPublishedContent);

    public object ConvertValue(Type type, string value) {
        if (!UdiParser.TryParse(value, out GuidUdi udi)) {
            return null;
        }

        var key = udi.Guid;
        
        var alias = ContentTypeAliases.GetOrAdd(udi.Guid, () => {
            var contentType = _contentTypeService.Get(key);
            
            return contentType.Alias;
        });

        return _contentTypeService.Get(alias);
    }
    
    private DataListItem ToDataListItem(IContentType contentType) {
        var guidUdi = contentType.GetUdi().ToString();
        
        var dataListItem = new DataListItem();
        dataListItem.Name = contentType.Name;
        dataListItem.Description = guidUdi;
        dataListItem.Icon = contentType.Icon;
        dataListItem.Value = guidUdi;
        dataListItem.Group = "N3O";

        return dataListItem;
    }
}