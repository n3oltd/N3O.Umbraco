using N3O.Umbraco.Content;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.PayPal.Content;

namespace N3O.Umbraco.Payments.PayPal {
    public class PayPalPaymentMethod : PaymentMethod {
        public PayPalPaymentMethod() : base("payPal", "PayPal", false, null, null) { }
        
        public override bool IsAvailable(IContentCache contentCache, Country country, Currency currency) {
            var settings = contentCache.Single<PayPalSettingsContent>();
            
            if (settings == null) {
                return false;
            }
            
            return true;
        }
    }
}