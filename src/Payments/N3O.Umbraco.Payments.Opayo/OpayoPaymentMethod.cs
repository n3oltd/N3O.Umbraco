using N3O.Umbraco.Content;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Opayo.Content;
using N3O.Umbraco.Payments.Opayo.Models;

namespace N3O.Umbraco.Payments.Opayo {
    public class OpayoPaymentMethod : PaymentMethod {
        public OpayoPaymentMethod() : base("opayo", "Opayo", true, typeof(OpayoPayment), typeof(OpayoCredential)) { }

        public override bool IsAvailable(IContentCache contentCache, Country country, Currency currency) {
            var settings = contentCache.Single<OpayoSettingsContent>();
            
            if (settings == null) {
                return false;
            }
            
            return true;
        }
    }
}