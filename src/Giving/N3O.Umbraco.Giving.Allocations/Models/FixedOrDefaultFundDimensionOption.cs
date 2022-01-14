using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Models {
    public class FixedOrDefaultFundDimensionOption<T> : Value where T : FundDimensionOption<T> {
        public FixedOrDefaultFundDimensionOption(FundDimensionOption<T> @fixed, FundDimensionOption<T> @default) {
            Fixed = @fixed;
            Default = @default;
        }

        public FundDimensionOption<T> Fixed { get; }
        public FundDimensionOption<T> Default { get; }
    }
}
