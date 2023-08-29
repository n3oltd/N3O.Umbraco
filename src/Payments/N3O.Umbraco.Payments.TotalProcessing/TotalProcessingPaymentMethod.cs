using N3O.Umbraco.Content;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.TotalProcessing.Content;
using N3O.Umbraco.Payments.TotalProcessing.Models;

namespace N3O.Umbraco.Payments.TotalProcessing;

public class TotalProcessingPaymentMethod : PaymentMethod {
    public TotalProcessingPaymentMethod() 
        : base("totalProcessing",
               "TotalProcessing",
               typeof(TotalProcessingPayment),
               typeof(TotalProcessingCredential)) { }
    
    public override string GetSettingsContentTypeAlias() {
        return AliasHelper<TotalProcessingSettingsContent>.ContentTypeAlias();
    }
}
