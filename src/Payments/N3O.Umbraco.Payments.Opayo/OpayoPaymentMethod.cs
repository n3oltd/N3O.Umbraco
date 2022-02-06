using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Opayo.Models;

namespace N3O.Umbraco.Payments.Opayo {
    public class OpayoPaymentMethod : PaymentMethod {
        public OpayoPaymentMethod() : base("opayo", "Opayo", true, typeof(OpayoPayment), typeof(OpayoCredential)) { }
    }
}