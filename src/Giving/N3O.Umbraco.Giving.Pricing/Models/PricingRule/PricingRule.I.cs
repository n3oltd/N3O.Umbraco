using N3O.Umbraco.FundDimensions;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Pricing.Models {
    public interface IPricingRule : IPrice {
        IEnumerable<FundDimension1Option> Dimension1Options { get; }
        IEnumerable<FundDimension2Option> Dimension2Options { get; }
        IEnumerable<FundDimension3Option> Dimension3Options { get; }
        IEnumerable<FundDimension4Option> Dimension4Options { get; }
    }
}