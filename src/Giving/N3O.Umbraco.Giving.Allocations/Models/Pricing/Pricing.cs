using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class Pricing : Value, IPricing {
    [JsonConstructor]
    public Pricing(Price price, IEnumerable<PricingRule> rules) {
        Price = price;
        Rules = rules;
    }

    public Pricing(IPricing pricing)
        : this(pricing.Price, pricing.Rules) { }
    
    public Pricing(IPrice price, IEnumerable<IPricingRule> pricingRules)
        : this(price.IfNotNull(x => new Price(x)),
               pricingRules.OrEmpty().Select(x => new PricingRule(x)).ToList()) { }

    public Price Price { get; }
    public IEnumerable<PricingRule> Rules { get; }

    [JsonIgnore]
    IPrice IPricing.Price => Price;

    [JsonIgnore]
    IEnumerable<IPricingRule> IPricing.Rules => Rules;
}