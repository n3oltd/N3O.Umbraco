using N3O.Umbraco.Entities;
using Newtonsoft.Json;
using Umbraco.Extensions;

namespace N3O.Umbraco.Subscription;

public class SubscriptionInfo : Value {
    [JsonConstructor]
    public SubscriptionInfo(EntityId id, string region, int number) {
        Id = id;
        Region = region;
        Number = number;
    }

    public SubscriptionInfo(int number, string dataRegion)
        : this(number.ToGuid(), dataRegion, number) { }

    public EntityId Id { get; }
    public string Region { get; }
    public int Number { get; }
}