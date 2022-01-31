using System.Collections.Generic;

namespace N3O.Umbraco.FundDimensions {
    public interface IHoldFundDimensionOptions {
        IEnumerable<FundDimension1Option> Dimension1Options { get; }
        IEnumerable<FundDimension2Option> Dimension2Options { get; }
        IEnumerable<FundDimension3Option> Dimension3Options { get; }
        IEnumerable<FundDimension4Option> Dimension4Options { get; }
    }
}
