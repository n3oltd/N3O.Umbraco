using N3O.Umbraco.Content;

namespace N3O.Umbraco.Payments.GoCardless.Content {
    public class GoCardlessSettings : UmbracoContent<GoCardlessSettings> {
        public string ProductionAccessToken => GetValue(x => x.ProductionAccessToken);
        public string SandboxAccessToken => GetValue(x => x.SandboxAccessToken);
        public string TransactionDescription => GetValue(x => x.TransactionDescription);
    }
}