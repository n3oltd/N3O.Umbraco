using N3O.Giving.Models;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Models;

public class PricingRuleRes : PriceRes, IPricingRule {
    public FundDimensionValuesRes FundDimensions { get; set; }
    
    [JsonIgnore]
    IFundDimensionValues IPricingRule.FundDimensions => FundDimensions;
}
