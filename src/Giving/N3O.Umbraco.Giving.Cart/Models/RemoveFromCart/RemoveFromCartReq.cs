using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Cart.Models {
    public class RemoveFromCartReq {
        [Name("Donation Type")]
        public DonationType DonationType { get; set; }

        [Name("Index")]
        public int? Index { get; set; }
    }
}
