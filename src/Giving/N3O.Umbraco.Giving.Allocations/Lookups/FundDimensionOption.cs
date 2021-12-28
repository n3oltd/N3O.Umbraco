using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Lookups {
    public class FundDimensionOption : LookupContent {
        public bool IsUnrestricted => GetValue<FundDimensionOption, bool>(x => x.IsUnrestricted);
    }
}