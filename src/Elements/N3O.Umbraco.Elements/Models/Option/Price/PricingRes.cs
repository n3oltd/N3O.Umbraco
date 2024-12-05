using System.Collections.Generic;

namespace N3O.Umbraco.Elements.Models;

public class PricingData : PriceData {
    public IEnumerable<PricingRuleData> PriceRules { get; set; }
}
