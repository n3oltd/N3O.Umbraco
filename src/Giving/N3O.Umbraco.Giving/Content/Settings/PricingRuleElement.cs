using N3O.Giving.Models;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Models;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Content;

public class PricingRuleElement : UmbracoElement<PricingRuleElement>, IPricingRule, IFundDimensionValues {
    [UmbracoProperty("priceAmount")]
    public decimal Amount => GetValue(x => x.Amount);
    
    [UmbracoProperty("priceLocked")]
    public bool Locked => GetValue(x => x.Locked);
    
    public FundDimension1Value Dimension1 => GetAs(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetAs(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetAs(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetAs(x => x.Dimension4);

    [JsonIgnore]
    IFundDimensionValues IPricingRule.FundDimensions => new FundDimensionValues(this);
}
