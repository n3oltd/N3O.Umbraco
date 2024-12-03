using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Elements.Lookups;

public class SponsorshipComponent : LookupContent<SponsorshipComponent>, IPricing {
    public override string Id {
        get {
            var scheme = GetScheme();
            var baseId = base.Id;

            return ToUniqueId($"{scheme.Id}_{baseId}", Content().Key);
        }
    }
    
    public bool Mandatory => GetValue(x => x.Mandatory);
    public PriceContent Price => Content().As<PriceContent>();
    public IEnumerable<PricingRuleElement> PriceRules => GetNestedAs(x => x.PriceRules);

    [JsonIgnore]
    decimal IPrice.Amount => Price.Amount;

    [JsonIgnore]
    bool IPrice.Locked => Price.Locked;
    
    [JsonIgnore]
    IEnumerable<IPricingRule> IPricing.Rules => PriceRules;
    
    public SponsorshipScheme GetScheme() => Content().Parent.As<SponsorshipScheme>();
}
