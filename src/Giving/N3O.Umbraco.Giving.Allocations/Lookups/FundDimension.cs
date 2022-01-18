using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Lookups {
    public abstract class FundDimension<T, TOption> : LookupContent<T> where T : FundDimension<T, TOption> {
        public bool IsActive => GetValue(x => x.IsActive);
        public IReadOnlyList<TOption> Options => Content.Children.As<TOption>();
    }
    
    public class FundDimension1 : FundDimension<FundDimension1, FundDimension1Option> { }
    public class FundDimension2 : FundDimension<FundDimension2, FundDimension2Option> { }
    public class FundDimension3 : FundDimension<FundDimension3, FundDimension3Option> { }
    public class FundDimension4 : FundDimension<FundDimension4, FundDimension4Option> { }
}