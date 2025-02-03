using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Crm.Engage.Models;

public class SubscriptionInfo : Value {
    [JsonConstructor]
    public SubscriptionInfo(SubscriptionId id, string region) {
        Id = id;
        Region = region;
    }

    public SubscriptionInfo(int number, string region) : this(SubscriptionId.FromNumber(number), region) { }

    public SubscriptionId Id { get; }
    public string Region { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
        yield return Region;
    }
}