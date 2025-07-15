using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using System;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class SponsorshipComponent : ContentOrPublishedLookup, IHoldPricing {
    public SponsorshipComponent(string id, string name, Guid? contentId, bool mandatory, Pricing pricing)
        : base(id, name, contentId) {
        Mandatory = mandatory;
        Pricing = pricing;
    }

    public bool Mandatory { get; }
    public Pricing Pricing { get; }

    [JsonIgnore]
    IPricing IHoldPricing.Pricing => Pricing;
}