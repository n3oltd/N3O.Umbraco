using Newtonsoft.Json;
using Umbraco.Extensions;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class PricingRule : Value, IPricingRule {
    [JsonConstructor]
    public PricingRule(Price price, FundDimensionValues fundDimensions) {
        Price = price;
        FundDimensions = fundDimensions;
    }

    public PricingRule(IPricingRule pricingRule) 
        : this(pricingRule.Price.IfNotNull(x => new Price(x)),
               pricingRule.FundDimensions.IfNotNull(x => new FundDimensionValues(x))) { }

    public Price Price { get; }
    public FundDimensionValues FundDimensions { get; }
    
    [JsonIgnore]
    IPrice IPricingRule.Price => Price;
    
    [JsonIgnore]
    IFundDimensionValues IPricingRule.FundDimensions => FundDimensions;
}