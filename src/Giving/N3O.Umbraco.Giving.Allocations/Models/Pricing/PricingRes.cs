using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class PricingRes {
    public PriceRes Price { get; set; }
    public IEnumerable<PricingRuleRes> PriceRules { get; set; }
}
