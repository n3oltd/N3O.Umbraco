using N3O.Umbraco.Extensions;
using N3O.Umbraco.FundDimensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Giving.Pricing.Content;
using N3O.Umbraco.Giving.Pricing.Models;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Lookups {
    public class DonationItem :
        LookupContent<DonationItem>, IPricing, IHoldFundDimensionOptions, IHoldAllowedGivingTypes {
        public IEnumerable<GivingType> AllowedGivingTypes => GetValue(x => x.AllowedGivingTypes);
        public IEnumerable<FundDimension1Option> Dimension1Options => GetPickedAs(x => x.Dimension1Options);
        public IEnumerable<FundDimension2Option> Dimension2Options => GetPickedAs(x => x.Dimension2Options);
        public IEnumerable<FundDimension3Option> Dimension3Options => GetPickedAs(x => x.Dimension3Options);
        public IEnumerable<FundDimension4Option> Dimension4Options => GetPickedAs(x => x.Dimension4Options);
        public PriceContent Price => Content.As<PriceContent>();
        public IEnumerable<PricingRuleContent> PriceRules => GetNestedAs(x => x.PriceRules);

        [JsonIgnore]
        decimal IPrice.Amount => Price.Amount;

        [JsonIgnore]
        bool IPrice.Locked => Price.Locked;
        
        [JsonIgnore]
        IEnumerable<IPricingRule> IPricing.Rules => PriceRules;
    }
}
