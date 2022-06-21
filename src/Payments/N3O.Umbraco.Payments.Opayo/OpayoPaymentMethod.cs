using N3O.Umbraco.Content;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Opayo.Content;
using N3O.Umbraco.Payments.Opayo.Models;

namespace N3O.Umbraco.Payments.Opayo;

public class OpayoPaymentMethod : PaymentMethod {
    public OpayoPaymentMethod() : base("opayo", "Opayo", typeof(OpayoPayment), typeof(OpayoCredential)) { }
    
    public override string GetSettingsContentTypeAlias() {
        return AliasHelper<OpayoSettingsContent>.ContentTypeAlias();
    }
}
