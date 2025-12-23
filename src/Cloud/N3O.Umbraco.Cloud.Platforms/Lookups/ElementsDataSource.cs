using Humanizer;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Community.Contentment.DataEditors;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class ElementsDataSource : IDataPickerSource, IDataSourceValueConverter {
    private readonly ICdnClient _cdnClient;
    private readonly IContentLocator _contentLocator;
    
    public ElementsDataSource(ICdnClient cdnClient,
                              IContentLocator contentLocator) {
        _cdnClient = cdnClient;
        _contentLocator = contentLocator;
    }

    public string Name => "Platform Elements";
    public string Icon => "icon-item-arrangement";
    public string Group => "N3O";
    public string Description => "A list of Platform elements";
    public Dictionary<string, object> DefaultValues => default;
    public IEnumerable<ConfigurationField> Fields => default;
    public OverlaySize OverlaySize => OverlaySize.Small;
    
    public Task<IEnumerable<DataListItem>> GetItemsAsync(Dictionary<string, object> config,
                                                         IEnumerable<string> values) {
        if (values.Any()) {
            var allElements = _contentLocator.All(x => x.ContentType.CompositionAliases.Contains(AliasHelper<ElementContent>.ContentTypeAlias()));
            
            var items = new List<DataListItem>();
            
            foreach (var value in values) {
                var publishedElement = JsonConvert.DeserializeObject<PublishedElement>(value);
                var elementContent = allElements.Single(x => x.Key == Guid.Parse(publishedElement.Id));

                var dataListItem = ToDataListItem(publishedElement, elementContent);
                
                items.Add(dataListItem);
            }
            
            return Task.FromResult<IEnumerable<DataListItem>>(items);
        }

        return Task.FromResult(Enumerable.Empty<DataListItem>());
    }

    public async Task<PagedResult<DataListItem>> SearchAsync(Dictionary<string, object> config,
                                                             int pageNumber = 1,
                                                             int pageSize = 12,
                                                             string query = "") {
        var allElements = _contentLocator.All(x => x.ContentType.CompositionAliases.Contains(AliasHelper<ElementContent>.ContentTypeAlias()));
        
        var publishedElementsTasks = new List<Task<PublishedElement>>();
        
        allElements.Do(x => {
            var task = _cdnClient.DownloadPublishedContentAsync<PublishedElement>(PublishedFileKinds.Element,
                                                                                   $"{x.Key}.json",
                                                                                   JsonSerializers.Simple);
            
            publishedElementsTasks.Add(task);
        });
        
        var publishedElements = await Task.WhenAll(publishedElementsTasks);
        
        var totalRecords = -1L;
        var pageIndex = pageNumber - 1;
        var items = publishedElements.ExceptNull().ToList();

        if (items.Any()) {
            var offset = pageIndex * pageSize;
            
            var results = new PagedResult<DataListItem>(totalRecords, pageNumber, pageSize);
            
            var resultItems = new List<DataListItem>();
            
            foreach (var item in items.Skip(offset).Take(pageSize)) {
                var elementContent = allElements.Single(x => x.Key == Guid.Parse(item.Id));

                var dataListItem = ToDataListItem(item, elementContent);
                
                resultItems.Add(dataListItem);
            }
            
            results.Items = resultItems;

            return results;
        }

        return new PagedResult<DataListItem>(totalRecords, pageNumber, pageSize);
    }
    
    public Type GetValueType(Dictionary<string, object> config) => typeof(PublishedElement);

    public object ConvertValue(Type type, string value) {
        return JsonConvert.DeserializeObject<PublishedElement>(value);
    }
    
    private DataListItem ToDataListItem(PublishedElement publishedElement, IPublishedContent elementContent) {
        var dataListItem = new DataListItem();
        dataListItem.Name = elementContent.Name;
        dataListItem.Description = publishedElement.Type.ToString().Humanize();
        dataListItem.Icon = "icon-donate";
        dataListItem.Value = JsonConvert.SerializeObject(publishedElement);
        dataListItem.Group = "N3O";

        return dataListItem;
    }
}