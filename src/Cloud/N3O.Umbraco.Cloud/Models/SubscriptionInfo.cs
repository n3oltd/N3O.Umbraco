using N3O.Umbraco.Cloud.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class SubscriptionInfo : Value {
    [JsonConstructor]
    public SubscriptionInfo(DataRegion dataRegion, SubscriptionId subscriptionId) {
        DataRegion = dataRegion;
        SubscriptionId = subscriptionId;
    }

    public DataRegion DataRegion { get; }
    public SubscriptionId SubscriptionId { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return DataRegion;
        yield return SubscriptionId;
    }
}