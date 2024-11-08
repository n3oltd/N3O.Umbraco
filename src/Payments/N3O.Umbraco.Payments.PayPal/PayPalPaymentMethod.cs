using N3O.Umbraco.Content;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.PayPal.Content;
using N3O.Umbraco.Payments.PayPal.Models;

namespace N3O.Umbraco.Payments.PayPal;

public class PayPalPaymentMethod : PaymentMethod {
    public PayPalPaymentMethod() : base("payPal", "PayPal", typeof(PayPalPayment), typeof(PayPalCredential)) { }
    
    public override string GetSettingsContentTypeAlias() {
        return AliasHelper<PayPalSettingsContent>.ContentTypeAlias();
    }
}
