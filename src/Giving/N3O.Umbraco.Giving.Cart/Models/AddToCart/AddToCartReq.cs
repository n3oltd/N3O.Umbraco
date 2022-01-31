using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Cart.Models {
    public class AddToCartReq {
        [Name("Giving Type")]
        public GivingType GivingType { get; set; }

        [Name("Allocation")]
        public AllocationReq Allocation { get; set; }
    
        [Name("Quantity")]
        public int? Quantity { get; set; }
    }
}
