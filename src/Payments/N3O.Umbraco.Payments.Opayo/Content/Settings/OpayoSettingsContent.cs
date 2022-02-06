using N3O.Umbraco.Payments.Content;

namespace N3O.Umbraco.Payments.Opayo.Content {
    public class OpayoSettingsContent : PaymentMethodSettingsContent<OpayoSettingsContent> {
        public string ProductionIntegrationKey => GetValue(x => x.ProductionIntegrationKey);
        public string ProductionIntegrationPassword => GetValue(x => x.ProductionIntegrationPassword);
        public string ProductionVendorName => GetValue(x => x.ProductionVendorName);

        public string StagingIntegrationKey => GetValue(x => x.StagingIntegrationKey);
        public string StagingIntegrationPassword => GetValue(x => x.StagingIntegrationPassword);
        public string StagingVendorName => GetValue(x => x.StagingVendorName);
    }
}