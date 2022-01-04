using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Payments.Lookups {
    public class PaymentObjectType : NamedLookup {
        public PaymentObjectType(string id, string name) : base(id, name) { }
    }

    public class PaymentObjectTypes : StaticLookupsCollection<PaymentObjectType> {
        public static readonly PaymentObjectType Credential = new("credential", "Credential");
        public static readonly PaymentObjectType Payment = new("payment", "Payment");
    }
}