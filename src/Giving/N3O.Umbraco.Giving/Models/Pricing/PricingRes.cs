using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models {
    public class PricingRes : PriceRes {
        public IEnumerable<PricingRuleRes> PriceRules { get; set; }
    }
}