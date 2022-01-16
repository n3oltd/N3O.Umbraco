using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations {
    public interface IFundStructure {
        FundDimension1Content Dimension1 { get; }
        FundDimension2Content Dimension2 { get; }
        FundDimension3Content Dimension3 { get; }
        FundDimension4Content Dimension4 { get; }
    
        IEnumerable<FundDimensionContent> Dimensions { get; }

        IReadOnlyList<T> GetOptions<T>() where T : FundDimensionOption<T>;
    }
}
