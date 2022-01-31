using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.FundDimensions;
using N3O.Umbraco.Giving.Pricing.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Pricing.Content {
    public class PricingRuleContent : UmbracoContent<PricingRuleContent>, IPricingRule {
        public PriceContent Price => Content.As<PriceContent>();
        public IEnumerable<FundDimension1Option> Dimension1Options => GetPickedAs(x => x.Dimension1Options);
        public IEnumerable<FundDimension2Option> Dimension2Options => GetPickedAs(x => x.Dimension2Options);
        public IEnumerable<FundDimension3Option> Dimension3Options => GetPickedAs(x => x.Dimension3Options);
        public IEnumerable<FundDimension4Option> Dimension4Options => GetPickedAs(x => x.Dimension4Options);

        [JsonIgnore]
        decimal IPrice.Amount => Price.Amount;

        [JsonIgnore]
        bool IPrice.Locked => Price.Locked;
    }
}