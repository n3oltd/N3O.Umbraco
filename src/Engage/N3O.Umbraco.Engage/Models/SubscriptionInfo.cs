using N3O.Umbraco.Engage.Lookups;
using N3O.Umbraco.Entities;
using Newtonsoft.Json;
using Umbraco.Extensions;

namespace N3O.Umbraco.Engage.Models;

public class SubscriptionInfo : Value {
    [JsonConstructor]
    public SubscriptionInfo(Guid contentId, EntityId id, DataRegion region, int number) {
        ContentId = contentId;
        Id = id;
        Region = region;
        Number = number;
    }

    public SubscriptionInfo(Guid contentId, int number, DataRegion dataRegion)
        : this(contentId, number.ToGuid(), dataRegion, number) { }
    
    public Guid ContentId { get; }
    public EntityId Id { get; }
    public DataRegion Region { get; }
    public int Number { get; }
}