using N3O.Umbraco.Payments.Content;

namespace N3O.Umbraco.Payments.GoCardless.Content {
    public class GoCardlessSettingsContent : PaymentMethodSettingsContent<GoCardlessSettingsContent> {
        public string ProductionAccessToken => GetValue(x => x.ProductionAccessToken);
        public string SandboxAccessToken => GetValue(x => x.SandboxAccessToken);
    }
}