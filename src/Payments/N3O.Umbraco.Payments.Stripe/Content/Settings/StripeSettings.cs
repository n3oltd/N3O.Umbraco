using N3O.Umbraco.Content;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public class StripeSettings : UmbracoContent {
        public string ProductionClientKey => GetValue<StripeSettings, string>(x => x.ProductionClientKey);
        public string ProductionSecretKey => GetValue<StripeSettings, string>(x => x.ProductionSecretKey);
        public string SandboxClientKey => GetValue<StripeSettings, string>(x => x.SandboxClientKey);
        public string SandboxSecretKey => GetValue<StripeSettings, string>(x => x.SandboxSecretKey);
    }
}