using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models {
    public interface IHoldFundDimensionOptions {
        IEnumerable<FundDimension1Option> Dimension1Options { get; }
        IEnumerable<FundDimension2Option> Dimension2Options { get; }
        IEnumerable<FundDimension3Option> Dimension3Options { get; }
        IEnumerable<FundDimension4Option> Dimension4Options { get; }
    }
}
