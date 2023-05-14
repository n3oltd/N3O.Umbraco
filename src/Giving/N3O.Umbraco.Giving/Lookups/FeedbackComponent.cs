using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Lookups;

public class FeedbackComponent : LookupContent<FeedbackComponent>, IPricing {
    public bool Mandatory => GetValue(x => x.Mandatory);
    public PriceContent Price => Content().As<PriceContent>();
    public IEnumerable<PricingRuleElement> PriceRules => GetNestedAs(x => x.PriceRules);

    [JsonIgnore]
    decimal IPrice.Amount => Price.Amount;

    [JsonIgnore]
    bool IPrice.Locked => Price.Locked;
    
    [JsonIgnore]
    IEnumerable<IPricingRule> IPricing.Rules => PriceRules;
    
    public FeedbackScheme GetScheme() => Content().Parent.As<FeedbackScheme>();

    protected override string GetId() {
        var scheme = GetScheme();
        var baseId = base.GetId();

        return $"{scheme.Id}_{baseId}";
    }
}
