using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.PayPal.Models;

namespace N3O.Umbraco.Payments.PayPal {
    public class PayPalPaymentMethod : PaymentMethod {
        public PayPalPaymentMethod() : base("payPal", "PayPal", false, typeof(PayPalPayment), null) { }
    }
}