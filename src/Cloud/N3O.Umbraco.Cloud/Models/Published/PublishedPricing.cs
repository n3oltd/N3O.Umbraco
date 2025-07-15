using N3O.Umbraco.Giving.Allocations.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedPricing : IPricing {
    public PublishedPrice Price { get; set; }
    public IEnumerable<PublishedPricingRule> Rules { get; set; }

    [JsonIgnore]
    IPrice IPricing.Price => Price;
    
    [JsonIgnore]
    IEnumerable<IPricingRule> IPricing.Rules => Rules;

    public Pricing GetPricing() {
        return new Pricing(Price.GetPrice(), Rules.Select(x => x.GetPricingRule()));
    }
}
