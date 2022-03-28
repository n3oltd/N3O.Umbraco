using N3O.Umbraco.Payments.Content;

namespace N3O.Umbraco.Payments.Bambora.Content {
    public class BamboraSettingsContent : PaymentMethodSettingsContent<BamboraSettingsContent> {
        public string ProductionMerchantId => GetValue(x => x.ProductionMerchantId);
        public string ProductionPaymentPasscode => GetValue(x => x.ProductionPaymentPasscode);
        public string ProductionProfilePasscode => GetValue(x => x.ProductionProfilePasscode);

        public string StagingMerchantId => GetValue(x => x.StagingMerchantId);
        public string StagingPaymentPasscode => GetValue(x => x.StagingPaymentPasscode);
        public string StagingProfilePasscode => GetValue(x => x.StagingProfilePasscode);
    }
}