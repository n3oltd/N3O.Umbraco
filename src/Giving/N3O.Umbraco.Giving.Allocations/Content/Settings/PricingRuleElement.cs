using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Models;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Allocations.Content;

public class PricingRuleElement : UmbracoElement<PricingRuleElement>, IPricingRule, IFundDimensionValues {
    [UmbracoProperty(AllocationsConstants.Aliases.PricingRule.Properties.Amount)]
    public decimal Amount => GetValue(x => x.Amount);
    
    [UmbracoProperty(AllocationsConstants.Aliases.PricingRule.Properties.Locked)]
    public bool Locked => GetValue(x => x.Locked);
    
    public FundDimension1Value Dimension1 => GetAs(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetAs(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetAs(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetAs(x => x.Dimension4);
    
    IFundDimensionValues IPricingRule.FundDimensions => new FundDimensionValues(this);

    [JsonIgnore]
    public Price Price => Amount > 0 ? new Price(Amount, Locked) : null;

    [JsonIgnore]
    IPrice IPricingRule.Price => Price;
}
