using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class FeedbackScheme :
    LookupContent<FeedbackScheme>, IPricing, IFundDimensionsOptions, IHoldAllowedGivingTypes {
    public IEnumerable<GivingType> AllowedGivingTypes => GetValue(x => x.AllowedGivingTypes);
    public IEnumerable<FeedbackCustomFieldDefinitionElement> CustomFields => GetNestedAs(x => x.CustomFields);
    public IEnumerable<FundDimension1Value> Dimension1Options => GetPickedAs(x => x.Dimension1Options);
    public IEnumerable<FundDimension2Value> Dimension2Options => GetPickedAs(x => x.Dimension2Options);
    public IEnumerable<FundDimension3Value> Dimension3Options => GetPickedAs(x => x.Dimension3Options);
    public IEnumerable<FundDimension4Value> Dimension4Options => GetPickedAs(x => x.Dimension4Options);
    public PriceContent Price => Content().As<PriceContent>();
    public IEnumerable<PricingRuleElement> PriceRules => GetNestedAs(x => x.PriceRules);
    
    [JsonIgnore]
    decimal IPrice.Amount => Price.Amount;

    [JsonIgnore]
    bool IPrice.Locked => Price.Locked;
    
    [JsonIgnore]
    IEnumerable<IPricingRule> IPricing.Rules => PriceRules;
}