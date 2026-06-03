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
    private readonly IPublishedContentTypeFactory _publishedContentTypeFactory;

    public ContentTypesDataSource(IContentTypeService contentTypeService,
                                  IPublishedContentTypeFactory publishedContentTypeFactory) {
        _contentTypeService = contentTypeService;
        _publishedContentTypeFactory = publishedContentTypeFactory;
    }

    public string Name => "Umbraco Content Types";
    public string Icon => "icon-item-arrangement";
    public string Group => "N3O";
    public string Description => "A list of Umbraco content types";
    public Dictionary<string, object> DefaultValues => default;
    public IEnumerable<ContentmentConfigurationField> Fields => default;
    public OverlaySize OverlaySize => OverlaySize.Small;

    public IEnumerable<DataListItem> GetItems(Dictionary<string, object> config) {
        return _contentTypeService.GetAll().Select(ToDataListItem).ToList();
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
        var items = _contentTypeService.GetAll();

        if (query.HasValue()) {
            items = items.Where(x => x.Name.InvariantContains(query) || x.Alias.InvariantContains(query));
        }

        var allItems = items.ToList();
        var offset = (pageNumber - 1) * pageSize;

        var result = new PagedViewModel<DataListItem> {
            Total = allItems.Count,
            Items = allItems.Skip(offset).Take(pageSize).Select(ToDataListItem)
        };

        return Task.FromResult(result);
    }

    public Type GetValueType(Dictionary<string, object> config) => typeof(IPublishedContent);

    public object ConvertValue(Type type, string value) {
        if (!UdiParser.TryParse(value, out GuidUdi udi)) {
            return default;
        }

        var key = udi.Guid;

        var alias = ContentTypeAliases.GetOrAdd(udi.Guid, () => {
            var contentType = _contentTypeService.Get(key);

            return contentType.Alias;
        });

        var contentType = _contentTypeService.Get(alias);
        return contentType != null ? _publishedContentTypeFactory.CreateContentType(contentType) : default;
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
