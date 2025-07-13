using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class FeedbackScheme :
    LookupContent<FeedbackScheme>, IHoldAllowedGivingTypes, IHoldFundDimensionOptions, IHoldPricing {
    public IEnumerable<GivingType> AllowedGivingTypes => GetValue(x => x.AllowedGivingTypes);
    public IEnumerable<FeedbackCustomFieldDefinitionElement> CustomFields => GetNestedAs(x => x.CustomFields);

    [UmbracoProperty(AllocationsConstants.Aliases.FeedbackScheme.Properties.Dimension1)]
    public IEnumerable<FundDimension1Value> Dimension1 => GetPickedAs(x => x.Dimension1);
    
    [UmbracoProperty(AllocationsConstants.Aliases.FeedbackScheme.Properties.Dimension2)]
    public IEnumerable<FundDimension2Value> Dimension2 => GetPickedAs(x => x.Dimension2);
    
    [UmbracoProperty(AllocationsConstants.Aliases.FeedbackScheme.Properties.Dimension3)]
    public IEnumerable<FundDimension3Value> Dimension3 => GetPickedAs(x => x.Dimension3);
    
    [UmbracoProperty(AllocationsConstants.Aliases.FeedbackScheme.Properties.Dimension4)]
    public IEnumerable<FundDimension4Value> Dimension4 => GetPickedAs(x => x.Dimension4);
    
    public PriceContent Price => Content().As<PriceContent>();
    
    [UmbracoProperty(AllocationsConstants.Aliases.FeedbackScheme.Properties.PricingRules)]
    public IEnumerable<PricingRuleElement> PricingRules => GetNestedAs(x => x.PricingRules);
    
    [JsonIgnore]
    public FundDimensionOptions FundDimensionOptions => new(Dimension1, Dimension2, Dimension3, Dimension4);

    [JsonIgnore]
    public Pricing Pricing {
        get {
            var price = Price?.Amount > 0 ? new Price(Price.Amount, Price.Locked) : null;
            
            if (price ==  null && PricingRules.None()) {
                return null;
            } else {
                return new Pricing(price, PricingRules);
            }
        }
    }
    
    [JsonIgnore]
    IFundDimensionOptions IHoldFundDimensionOptions.FundDimensionOptions => FundDimensionOptions;
    
    [JsonIgnore]
    IPricing IHoldPricing.Pricing => Pricing;
}