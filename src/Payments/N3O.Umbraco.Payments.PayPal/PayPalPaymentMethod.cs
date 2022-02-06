using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments.PayPal {
    public class PayPalPaymentMethod : PaymentMethod {
        public PayPalPaymentMethod() : base("payPal", "PayPal", false, null, null) { }
    }
}