using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Giving.Models {
    public abstract class FundDimension<T, TValue> : LookupContent<T> where T : FundDimension<T, TValue> {
        public bool IsActive => GetValue(x => x.IsActive);
        public IReadOnlyList<TValue> Options => Content.Children.As<TValue>();
    }
    
    public class FundDimension1 : FundDimension<FundDimension1, FundDimension1Value> { }
    public class FundDimension2 : FundDimension<FundDimension2, FundDimension2Value> { }
    public class FundDimension3 : FundDimension<FundDimension3, FundDimension3Value> { }
    public class FundDimension4 : FundDimension<FundDimension4, FundDimension4Value> { }
}