using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class SubscriptionInfo : Value {
    [JsonConstructor]
    public SubscriptionInfo(string dataRegion, SubscriptionId id) {
        DataRegion = dataRegion;
        Id = id;
    }

    public string DataRegion { get; }
    public SubscriptionId Id { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return DataRegion;
        yield return Id;
    }
}