using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class PricingRes : PriceRes {
    public IEnumerable<PricingRuleRes> PriceRules { get; set; }
}
