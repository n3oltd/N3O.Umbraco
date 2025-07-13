using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class PricingRuleRes : IPricingRule {
    public PriceRes Price { get; set; }
    public FundDimensionValuesRes FundDimensions { get; set; }
    
    [JsonIgnore]
    IPrice IPricingRule.Price => Price;
    
    [JsonIgnore]
    IFundDimensionValues IPricingRule.FundDimensions => FundDimensions;
}
