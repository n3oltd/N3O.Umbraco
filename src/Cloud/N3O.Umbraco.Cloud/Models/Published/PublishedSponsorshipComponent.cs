using Humanizer;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using Newtonsoft.Json;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedSponsorshipComponent : IHoldPricing {
    public string Name { get; set; }
    public bool Required { get; set; }
    public PublishedPricing Pricing { get; set; }

    public SponsorshipComponent GetSponsorshipComponent() {
        return new SponsorshipComponent(Name.Camelize(), Name, null, Required, Pricing.GetPricing());
    }
    
    [JsonIgnore]
    IPricing IHoldPricing.Pricing => Pricing;
}
