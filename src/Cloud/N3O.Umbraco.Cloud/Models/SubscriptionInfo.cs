using N3O.Umbraco.Cloud.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class SubscriptionInfo : Value {
    [JsonConstructor]
    public SubscriptionInfo(DataRegion dataRegion, SubscriptionDescriptor id) {
        DataRegion = dataRegion;
        Id = id;
    }

    public DataRegion DataRegion { get; }
    public SubscriptionDescriptor Id { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return DataRegion;
        yield return Id;
    }
}