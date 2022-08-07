using N3O.Umbraco.Content;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Worldline.Content;
using N3O.Umbraco.Payments.Worldline.Models;

namespace N3O.Umbraco.Payments.Worldline;

public class WorldlinePaymentMethod : PaymentMethod {
    public WorldlinePaymentMethod() : base("worldline", "Worldline", typeof(WorldlinePayment), typeof(WorldlineCredential)) { }
    
    public override string GetSettingsContentTypeAlias() {
        return AliasHelper<WorldlineSettingsContent>.ContentTypeAlias();
    }
}
