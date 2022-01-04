using N3O.Umbraco.Content;

namespace N3O.Umbraco.Payments.PayPal.Content {
    public class PayPalSettings : UmbracoContent {
        public string ProductionClientId => GetValue<PayPalSettings, string>(x => x.ProductionClientId);
        public string SandboxClientId => GetValue<PayPalSettings, string>(x => x.SandboxClientId);
    }
}