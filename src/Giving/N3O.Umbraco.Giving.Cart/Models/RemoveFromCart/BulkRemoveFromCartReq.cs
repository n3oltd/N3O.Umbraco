using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Cart.Models;

public class BulkRemoveFromCartReq {
    [Name("Giving Type")]
    public GivingType GivingType { get; set; }

    [Name("Indexes")]
    public IEnumerable<int> Indexes { get; set; }
}
