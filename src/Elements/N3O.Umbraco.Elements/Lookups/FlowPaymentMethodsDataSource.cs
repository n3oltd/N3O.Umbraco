using N3O.Umbraco.Elements.Models;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Community.Contentment.DataEditors;
using Umbraco.Extensions;
using Type = System.Type;

namespace N3O.Umbraco.Elements.Lookups;

public class FlowPaymentMethodsDataSource : IDataPickerSource, IDataSourceValueConverter {
    private static readonly ConcurrentDictionary<string, FlowPaymentMethod> PaymentMethods = new(StringComparer.InvariantCultureIgnoreCase);
    
    private readonly CdnUrlAccessor _cdnUrlAccessor;
    
    public FlowPaymentMethodsDataSource(CdnUrlAccessor cdnUrlAccessor) {
        _cdnUrlAccessor = cdnUrlAccessor;
    }

    public string Name => "Flow Payment Methods";
    public string Icon => "icon-item-arrangement";
    public string Group => "N3O";
    public string Description => "A list of flow payment methods";
    public Dictionary<string, object> DefaultValues => null;
    public IEnumerable<ConfigurationField> Fields => null;
    public OverlaySize OverlaySize => OverlaySize.Small;
    
    public async Task<IEnumerable<DataListItem>> GetItemsAsync(Dictionary<string, object> config,
                                                               IEnumerable<string> values) {
        var paymentMethods = await GetPaymentMethodsAsync();
        
        paymentMethods.Do(x => PaymentMethods.AddOrUpdate(x.Name, x, (_, _) => x));
        
        if (values.Any()) {
            var items = paymentMethods.Where(x => values.Contains(x.Name, true)).Select(ToDataListItem);
            
            return items;
        } else {
            return [];   
        }
    }
    
    public Type GetValueType(Dictionary<string, object> config) => typeof(FlowPaymentMethod);

    public async Task<PagedResult<DataListItem>> SearchAsync(Dictionary<string, object> config,
                                                             int pageNumber = 1,
                                                             int pageSize = 12,
                                                             string query = "") {
        var pageIndex = pageNumber - 1;
        
        var paymentMethods = await GetPaymentMethodsAsync();
        
        paymentMethods.Do(x => PaymentMethods.AddOrUpdate(x.Name, x, (_, _) => x));
        
        if (query.HasValue()) {
            paymentMethods = paymentMethods.Where(x => x.Name.InvariantContains(query)).ToList();
        }

        IEnumerable<DataListItem> items;
        
        if (paymentMethods.Any()) {
            var offset = pageIndex * pageSize;
            
            items = paymentMethods.Skip(offset).Take(pageSize).Select(ToDataListItem);
        } else {
            items = [];
        }
        
        var results = new PagedResult<DataListItem>(paymentMethods.Count, pageNumber, pageSize);
        results.Items = items;

        return results;
    }

    public object ConvertValue(Type type, string value) {
        return PaymentMethods.GetValueOrDefault(value);
    }
    
    private async Task<IReadOnlyList<FlowPaymentMethod>> GetPaymentMethodsAsync() {
        var url = _cdnUrlAccessor.GetUrl(ElementsConstants.Cdn.Paths.FlowPaymentMethods);
        
        using var httpClient = new HttpClient();
        var res = await httpClient.GetFromJsonAsync<FlowPaymentMethodsResultList>(url);
        
        return res.Items.ToList();
    }
    
    private DataListItem ToDataListItem(FlowPaymentMethod paymentMethodRes) {
        var dataListItem = new DataListItem();
        dataListItem.Group = "N3O";
        dataListItem.Name = paymentMethodRes.Name;
        dataListItem.Description = paymentMethodRes.Name;
        dataListItem.Icon = "icon-coin-euro color-black";
        dataListItem.Value = paymentMethodRes.Name;
        
        return dataListItem;
    }
}