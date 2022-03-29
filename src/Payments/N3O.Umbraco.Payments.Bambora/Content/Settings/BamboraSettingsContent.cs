using N3O.Umbraco.Payments.Content;

namespace N3O.Umbraco.Payments.Bambora.Content {
    public class BamboraSettingsContent : PaymentMethodSettingsContent<BamboraSettingsContent> {
        public string ProductionMerchantId => GetValue(x => x.ProductionMerchantId);
        public string ProductionPaymentsPasscode => GetValue(x => x.ProductionPaymentsPasscode);
        public string ProductionProfilesPasscode => GetValue(x => x.ProductionProfilesPasscode);

        public string StagingMerchantId => GetValue(x => x.StagingMerchantId);
        public string StagingPaymentsPasscode => GetValue(x => x.StagingPaymentsPasscode);
        public string StagingProfilesPasscode => GetValue(x => x.StagingProfilesPasscode);
    }
}