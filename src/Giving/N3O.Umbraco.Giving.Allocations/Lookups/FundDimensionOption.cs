using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Lookups {
    public abstract class FundDimensionOption<T> : LookupContent<T> where T : FundDimensionOption<T> {
        public bool IsUnrestricted => GetValue(x => x.IsUnrestricted);
    }
    
    public class FundDimension1Option : FundDimensionOption<FundDimension1Option> { }
    public class FundDimension2Option : FundDimensionOption<FundDimension2Option> { }
    public class FundDimension3Option : FundDimensionOption<FundDimension3Option> { }
    public class FundDimension4Option : FundDimensionOption<FundDimension4Option> { }
}