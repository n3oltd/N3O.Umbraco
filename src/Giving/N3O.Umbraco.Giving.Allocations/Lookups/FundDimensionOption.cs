using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Lookups {
    public class FundDimensionOption : LookupContent<FundDimensionOption> {
        public bool IsUnrestricted => GetValue(x => x.IsUnrestricted);
    }
}