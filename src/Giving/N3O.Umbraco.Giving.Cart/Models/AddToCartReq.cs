using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;

namespace N3O.Umbraco.Giving.Cart.Models;

public class AddToCartReq {
    [Name("Donation Type")]
    public DonationType DonationType { get; set; }

    [Name("Allocation")]
    public AllocationReq Allocation { get; set; }
    
    [Name("Quantity")]
    public int? Quantity { get; set; }
}
