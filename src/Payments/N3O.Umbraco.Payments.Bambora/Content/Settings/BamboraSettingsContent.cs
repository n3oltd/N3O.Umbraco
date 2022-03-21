using N3O.Umbraco.Payments.Content;

namespace N3O.Umbraco.Payments.Bambora.Content {
    public class BamboraSettingsContent : PaymentMethodSettingsContent<BamboraSettingsContent> {
        public string ProductionMerchantId => GetValue(x => x.ProductionMerchantId);
        public string ProductionPasscode => GetValue(x => x.ProductionPasscode);

        public string StagingMerchantId => GetValue(x => x.StagingMerchantId);
        public string StagingPasscode => GetValue(x => x.StagingPasscode);
    }
}