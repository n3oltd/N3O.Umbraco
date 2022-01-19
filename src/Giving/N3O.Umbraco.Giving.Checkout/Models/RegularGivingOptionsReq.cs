using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class RegularGivingOptionsReq {
        [Name("Collection Day")]
        public int? CollectionDay { get; set; }
    }
}