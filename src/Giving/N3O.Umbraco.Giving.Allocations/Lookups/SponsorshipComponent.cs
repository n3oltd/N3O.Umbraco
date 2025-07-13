using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class SponsorshipComponent : LookupContent<SponsorshipComponent>, IHoldPricing {
    public override string Id {
        get {
            var scheme = GetScheme();
            var baseId = base.Id;

            return LookupContent.ToUniqueId($"{scheme.Id}_{baseId}", Content().Key);
        }
    }
    
    public bool Mandatory => GetValue(x => x.Mandatory);
    public PriceContent Price => Content().As<PriceContent>();
    public IEnumerable<PricingRuleElement> PriceRules => GetNestedAs(x => x.PriceRules);

    [UmbracoProperty(AllocationsConstants.Aliases.SponsorshipScheme.Properties.PricingRules)]
    public IEnumerable<PricingRuleElement> PricingRules => GetNestedAs(x => x.PricingRules);
    
    [JsonIgnore]
    public Pricing Pricing {
        get {
            var price = Price?.Amount > 0 ? new Price(Price.Amount, Price.Locked) : null;
            
            if (price ==  null && PricingRules.None()) {
                return null;
            } else {
                return new Pricing(price, PricingRules);
            }
        }
    }
    
    [JsonIgnore]
    IPricing IHoldPricing.Pricing => Pricing;
    
    public SponsorshipScheme GetScheme() => Content().Parent.As<SponsorshipScheme>();
}
