using N3O.Umbraco.Content;
using N3O.Umbraco.Payments.Bambora.Content;
using N3O.Umbraco.Payments.Bambora.Models;
using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments.Bambora {
    public class BamboraPaymentMethod : PaymentMethod {
        public BamboraPaymentMethod() : base("bambora", "Bambora", typeof(BamboraPayment), typeof(BamboraCredential)) { }
        
        public override string GetSettingsContentTypeAlias() {
            return AliasHelper<BamboraSettingsContent>.ContentTypeAlias();
        }
    }
}