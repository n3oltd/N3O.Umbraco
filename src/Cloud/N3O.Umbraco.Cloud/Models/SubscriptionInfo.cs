using N3O.Umbraco.Cloud.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class SubscriptionInfo : Value {
    [JsonConstructor]
    public SubscriptionInfo(DataRegion dataRegion, SubscriptionDescriptor descriptor) {
        DataRegion = dataRegion;
        Descriptor = descriptor;
    }

    public DataRegion DataRegion { get; }
    public SubscriptionDescriptor Descriptor { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return DataRegion;
        yield return Descriptor;
    }
}