using N3O.Umbraco.Crm.Engage;
using N3O.Umbraco.Elements.Models;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Community.Contentment.DataEditors;
using Umbraco.Extensions;
using Type = System.Type;

namespace N3O.Umbraco.Elements.Lookups;

public class PaymentMethodsDataSource : IDataPickerSource, IDataSourceValueConverter {
    private static readonly ConcurrentDictionary<string, FlowPaymentMethod> PaymentMethods = new();
    private readonly int SubscriptionNumber;
    private readonly string CdnUrl;
    
    public PaymentMethodsDataSource(CdnUrlAccessor cdnUrlAccessor, ISubscriptionAccessor subscriptionAccessor) {
        CdnUrl = cdnUrlAccessor.Get();
        SubscriptionNumber = subscriptionAccessor.GetSubscription().Number;
    }

    public string Name => "Website Payment Methods";
    public string Icon => "icon-item-arrangement";
    public string Group => "N3O";
    public string Description => "A list of allowed website payment methods";
    public Dictionary<string, object> DefaultValues => default;
    public IEnumerable<ConfigurationField> Fields => default;
    public OverlaySize OverlaySize => OverlaySize.Small;
    
    public async Task<IEnumerable<DataListItem>> GetItemsAsync(Dictionary<string, object> config,
                                                               IEnumerable<string> values) {
        var paymentMethods = await GetPaymentMethods(CdnUrl, SubscriptionNumber);
        
        paymentMethods.Do(x => PaymentMethods.AddOrUpdate(x.Name, x, (_, _) => x));
        
        if (values.Any()) {
            var items = paymentMethods.Where(x => values.Contains(x.Name)).Select(ToDataListItem);
            
            return items;
        }

        return [];
    }

    public async Task<PagedResult<DataListItem>> SearchAsync(Dictionary<string, object> config,
                                                             int pageNumber = 1,
                                                             int pageSize = 12,
                                                             string query = "") {
        var totalRecords = -1L;
        var pageIndex = pageNumber - 1;
        
        var paymentMethods = await GetPaymentMethods(CdnUrl, SubscriptionNumber);
        
        paymentMethods.Do(x => PaymentMethods.AddOrUpdate(x.Name, x, (_, _) => x));
        
        if (query.HasValue()) {
            paymentMethods = paymentMethods.Where(x => x.Name.InvariantContains(query)).ToList();
        }
        
        if (paymentMethods.Any()) {
            var offset = pageIndex * pageSize;
            
            var results = new PagedResult<DataListItem>(totalRecords, pageNumber, pageSize);
            
            results.Items = paymentMethods.Skip(offset).Take(pageSize).Select(ToDataListItem);

            return results;
        }

        return new PagedResult<DataListItem>(totalRecords, pageNumber, pageSize);
    }

    public Type GetValueType(Dictionary<string, object> config) => typeof(FlowPaymentMethod);

    public object ConvertValue(Type type, string value) {
        var hasPaymentMethod = PaymentMethods.TryGetValue(value, out var paymentMethod);

        if (!hasPaymentMethod) {
            return null;
        }

        return paymentMethod;
    }
    
    public static async Task<IReadOnlyList<FlowPaymentMethod>> GetPaymentMethods(string cdnUrl,
                                                                                 int subscriptionNumber) {
        var url = ElementsConstants.FlowPaymentMethods.PaymentMethodsCdnUrl.FormatWith(cdnUrl, subscriptionNumber);
        
        using var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync(url);

        var flowMethodsResult = JsonConvert.DeserializeObject<FlowPaymentMethodsResultList>(response);
        
        return flowMethodsResult.Items.ToList();
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