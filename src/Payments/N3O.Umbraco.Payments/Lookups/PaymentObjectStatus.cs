using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Payments.Lookups {
    public class PaymentObjectStatus : NamedLookup {
        public PaymentObjectStatus(string id, string name) : base(id, name) { }
    }

    public class PaymentObjectStatuses : StaticLookupsCollection<PaymentObjectStatus> {
        public static readonly PaymentObjectStatus Complete = new("complete", "Complete");
        public static readonly PaymentObjectStatus Error = new("error", "Error");
        public static readonly PaymentObjectStatus InProgress = new("inProgress", "InProgress");
    }
}