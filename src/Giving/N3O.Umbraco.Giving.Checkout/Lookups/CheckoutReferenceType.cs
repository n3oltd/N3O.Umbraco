using N3O.Umbraco.Counters;

namespace N3O.Umbraco.Giving.Checkout.Lookups {
    public class CheckoutReferenceType : ReferenceType {
        public CheckoutReferenceType() : base("WC", 1_000_000) { }
    }
}