using N3O.Umbraco.Giving.Allocations.Models;
using Newtonsoft.Json;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedPricingRule : IPricingRule {
    public PublishedPrice Price { get; set; }
    public PublishedFundDimensionValues FundDimensions { get; set; }

    [JsonIgnore]
    IFundDimensionValues IPricingRule.FundDimensions => FundDimensions;
    
    [JsonIgnore]
    IPrice IPricingRule.Price => Price;
}
