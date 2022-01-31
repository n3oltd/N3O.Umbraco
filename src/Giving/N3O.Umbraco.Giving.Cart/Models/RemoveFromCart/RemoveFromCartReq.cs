using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Cart.Models {
    public class RemoveFromCartReq {
        [Name("Giving Type")]
        public GivingType GivingType { get; set; }

        [Name("Index")]
        public int? Index { get; set; }
    }
}
