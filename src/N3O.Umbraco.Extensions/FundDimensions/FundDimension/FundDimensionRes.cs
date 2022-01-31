using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.FundDimensions {
    public class FundDimensionRes : NamedLookupRes {
        public bool IsActive { get; set; }
        public IEnumerable<FundDimensionOptionRes> Options { get; set; }
    }
}