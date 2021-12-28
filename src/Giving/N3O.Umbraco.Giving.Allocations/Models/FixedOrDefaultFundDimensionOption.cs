using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FixedOrDefaultFundDimensionOption : Value {
    public FixedOrDefaultFundDimensionOption(FundDimensionOption @fixed, FundDimensionOption @default) {
        Fixed = @fixed;
        Default = @default;
    }

    public FundDimensionOption Fixed { get; }
    public FundDimensionOption Default { get; }
}
