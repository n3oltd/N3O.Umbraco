using N3O.Umbraco.Content;

namespace N3O.Umbraco.Payments.GoCardless.Content {
    public class GoCardlessSettings : UmbracoContent {
        public string ProductionAccessToken => GetValue<GoCardlessSettings, string>(x => x.ProductionAccessToken);
        public string SandboxAccessToken => GetValue<GoCardlessSettings, string>(x => x.SandboxAccessToken);
        public string TransactionDescription => GetValue<GoCardlessSettings, string>(x => x.TransactionDescription);
    }
}