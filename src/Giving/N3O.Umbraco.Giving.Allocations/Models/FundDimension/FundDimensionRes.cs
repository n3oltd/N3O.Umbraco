using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FundDimensionRes : NamedLookupRes {
    public int Index { get; set; }
    public bool IsActive { get; set; }
    public IEnumerable<FundDimensionValueRes> Options { get; set; }
}
