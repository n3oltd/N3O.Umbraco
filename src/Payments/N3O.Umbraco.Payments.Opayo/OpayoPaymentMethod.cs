using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments.Opayo {
    public class OpayoPaymentMethod : PaymentMethod {
        public OpayoPaymentMethod() : base("opayo", "Opayo", null, null) { }
    }
}