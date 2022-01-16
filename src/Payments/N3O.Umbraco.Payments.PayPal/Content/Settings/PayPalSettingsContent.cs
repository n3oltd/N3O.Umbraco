using N3O.Umbraco.Content;

namespace N3O.Umbraco.Payments.PayPal.Content {
    public class PayPalSettingsContent : UmbracoContent<PayPalSettingsContent> {
        public string ProductionClientId => GetValue(x => x.ProductionClientId);
        public string SandboxClientId => GetValue(x => x.SandboxClientId);
    }
}