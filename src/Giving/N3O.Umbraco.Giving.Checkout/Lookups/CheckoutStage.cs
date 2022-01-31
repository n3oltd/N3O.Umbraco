using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Checkout.Lookups {
    public class CheckoutStage : NamedLookup {
        public CheckoutStage(string id, string name, int order) : base(id, name) {
            Order = order;
        }
        
        public int Order { get; }
    }

    public class CheckoutStages : StaticLookupsCollection<CheckoutStage> {
        public static readonly CheckoutStage Account = new CheckoutStage("account", "Account", 0);
        public static readonly CheckoutStage Complete = new CheckoutStage("complete", "Complete", 3);
        public static readonly CheckoutStage Donation = new CheckoutStage("donation", "Donation", 2);
        public static readonly CheckoutStage RegularGiving = new CheckoutStage("regularGiving", "Regular Giving", 1);
    }
}