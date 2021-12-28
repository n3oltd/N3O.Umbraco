using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations {
    public interface IFundStructure {
        FundDimension1 Dimension1 { get; }
        FundDimension2 Dimension2 { get; }
        FundDimension3 Dimension3 { get; }
        FundDimension4 Dimension4 { get; }
    
        IEnumerable<FundDimension> Dimensions { get; }

        IReadOnlyList<T> GetOptions<T>() where T : FundDimensionOption;
    }
}
