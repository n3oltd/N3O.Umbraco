using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Checkout.Lookups {
    public class CheckoutStage : NamedLookup {
        public CheckoutStage(string id, string name) : base(id, name) { }
    }

    public class CheckoutStages : StaticLookupsCollection<CheckoutStage> {
        public static readonly CheckoutStage Account = new CheckoutStage("account", "Account");
        public static readonly CheckoutStage SinglePayment = new CheckoutStage("singlePayment", "Single Payment");
        public static readonly CheckoutStage RegularPayment = new CheckoutStage("regularPayment", "Regular Payment");
        public static readonly CheckoutStage Complete = new CheckoutStage("complete", "Complete");
    }
}