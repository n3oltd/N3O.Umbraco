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
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class OfferingsDataSource : IDataPickerSource, IDataSourceValueConverter {
    private readonly ICdnClient _cdnClient;
    private readonly IContentLocator _contentLocator;
    
    public OfferingsDataSource(ICdnClient cdnClient,
                               IContentLocator contentLocator) {
        _cdnClient = cdnClient;
        _contentLocator = contentLocator;
    }

    public string Name => "Platform Offerings";
    public string Icon => "icon-item-arrangement";
    public string Group => "N3O";
    public string Description => "A list of Platform offerings";
    public Dictionary<string, object> DefaultValues => default;
    public IEnumerable<ConfigurationField> Fields => default;
    public OverlaySize OverlaySize => OverlaySize.Small;
    
    public Task<IEnumerable<DataListItem>> GetItemsAsync(Dictionary<string, object> config,
                                                         IEnumerable<string> values) {
        if (values.Any()) {
            var allOfferings = _contentLocator.All(x => x.ContentType.CompositionAliases.Contains(AliasHelper<OfferingContent>.ContentTypeAlias()));
            
            var items = new List<DataListItem>();
            
            foreach (var value in values) {
                var publishedOffering = JsonConvert.DeserializeObject<PublishedOffering>(value);
                var offeringContent = allOfferings.Single(x => x.Key == Guid.Parse(publishedOffering.Id));

                var dataListItem = ToDataListItem(publishedOffering, offeringContent);
                
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
        var allOfferings = _contentLocator.All(x => x.ContentType.CompositionAliases.Contains(AliasHelper<OfferingContent>.ContentTypeAlias()));
        
        var publishedOfferingsTasks = new List<Task<PublishedOffering>>();
        
        allOfferings.Do(x => {
            var task = _cdnClient.DownloadPublishedContentAsync<PublishedOffering>(PublishedFileKinds.Offering,
                                                                                   $"{x.Key}.json",
                                                                                   JsonSerializers.Simple);
            
            publishedOfferingsTasks.Add(task);
        });
        
        var publishedOfferings = await Task.WhenAll(publishedOfferingsTasks);
        
        var totalRecords = -1L;
        var pageIndex = pageNumber - 1;
        var items = publishedOfferings.ExceptNull().ToList();

        if (query.HasValue()) {
            items = items.Where(x => x.Name.InvariantContains(query)).ToList();
        }

        if (items.Any()) {
            var offset = pageIndex * pageSize;
            
            var results = new PagedResult<DataListItem>(totalRecords, pageNumber, pageSize);
            
            var resultItems = new List<DataListItem>();
            
            foreach (var item in items.Skip(offset).Take(pageSize)) {
                var offeringContent = allOfferings.Single(x => x.Key == Guid.Parse(item.Id));

                var dataListItem = ToDataListItem(item, offeringContent);
                
                resultItems.Add(dataListItem);
            }
            
            results.Items = resultItems;

            return results;
        }

        return new PagedResult<DataListItem>(totalRecords, pageNumber, pageSize);
    }
    
    public Type GetValueType(Dictionary<string, object> config) => typeof(PublishedOffering);

    public object ConvertValue(Type type, string value) {
        return JsonConvert.DeserializeObject<PublishedOffering>(value);
    }
    
    private DataListItem ToDataListItem(PublishedOffering publishedOffering, IPublishedContent offeringContent) {
        var dataListItem = new DataListItem();
        dataListItem.Name = offeringContent.Name;
        dataListItem.Description = offeringContent.Name;
        dataListItem.Icon = publishedOffering.Icon.Markup;
        dataListItem.Value = JsonConvert.SerializeObject(publishedOffering);
        dataListItem.Group = "N3O";

        return dataListItem;
    }
}