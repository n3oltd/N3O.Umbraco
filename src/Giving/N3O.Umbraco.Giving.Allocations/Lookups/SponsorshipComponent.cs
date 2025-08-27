using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using System;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class SponsorshipComponent : ContentOrPublishedLookup, IHoldPricing {
    public SponsorshipComponent(string id,
                                string name,
                                Guid? contentId,
                                string schemeId,
                                bool mandatory,
                                Pricing pricing)
        : base($"{schemeId}_{id}", name, contentId) {
        SchemeId = schemeId;
        Mandatory = mandatory;
        Pricing = pricing;
    }

    public string SchemeId { get; }
    public bool Mandatory { get; }
    public Pricing Pricing { get; }

    [JsonIgnore]
    IPricing IHoldPricing.Pricing => Pricing;
}