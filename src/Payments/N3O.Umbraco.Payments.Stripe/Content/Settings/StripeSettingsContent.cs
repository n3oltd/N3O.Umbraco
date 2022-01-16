using N3O.Umbraco.Content;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public class StripeSettingsContent : UmbracoContent<StripeSettingsContent> {
        public string ProductionClientKey => GetValue(x => x.ProductionClientKey);
        public string ProductionSecretKey => GetValue(x => x.ProductionSecretKey);
        public string SandboxClientKey => GetValue(x => x.SandboxClientKey);
        public string SandboxSecretKey => GetValue(x => x.SandboxSecretKey);
    }
}