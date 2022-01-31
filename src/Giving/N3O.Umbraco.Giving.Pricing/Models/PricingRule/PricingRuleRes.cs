using N3O.Umbraco.FundDimensions;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Pricing.Models {
    public class PricingRuleRes : PriceRes, IPricingRule {
        public IEnumerable<FundDimension1Option> Dimension1Options { get; set; }
        public IEnumerable<FundDimension2Option> Dimension2Options { get; set; }
        public IEnumerable<FundDimension3Option> Dimension3Options { get; set; }
        public IEnumerable<FundDimension4Option> Dimension4Options { get; set; }
    }
}