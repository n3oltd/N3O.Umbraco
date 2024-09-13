using N3O.Umbraco.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using Umbraco.Extensions;

namespace N3O.Umbraco.Crm.Engage.Models;

public class SubscriptionInfo : Value {
    [JsonConstructor]
    public SubscriptionInfo(EntityId id, string region, int number) {
        Id = id;
        Region = region;
        Number = number;
    }

    public SubscriptionInfo(int number, string region) : this(number.ToGuid(), region, number) { }

    public EntityId Id { get; }
    public string Region { get; }
    public int Number { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
        yield return Region;
        yield return Number;
        
    }
}