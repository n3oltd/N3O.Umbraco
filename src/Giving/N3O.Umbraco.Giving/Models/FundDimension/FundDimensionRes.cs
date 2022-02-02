using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Giving.Models {
    public class FundDimensionRes : NamedLookupRes {
        public bool IsActive { get; set; }
        public IEnumerable<FundDimensionValueRes> Options { get; set; }
    }
}