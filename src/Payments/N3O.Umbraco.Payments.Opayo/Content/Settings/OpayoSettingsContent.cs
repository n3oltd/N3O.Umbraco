using N3O.Umbraco.Payments.Content;

namespace N3O.Umbraco.Payments.Opayo.Content {
    public class OpayoSettingsContent : PaymentMethodSettingsContent<OpayoSettingsContent> {
        public string ProductionIntegrationKey => GetValue(x => x.ProductionIntegrationKey);
        public string ProductionIntegrationPassword => GetValue(x => x.ProductionIntegrationPassword);
        public string ProductionVendorName => GetValue(x => x.ProductionVendorName);

        public string SandboxIntegrationKey => GetValue(x => x.SandboxIntegrationKey);
        public string SandboxIntegrationPassword => GetValue(x => x.SandboxIntegrationPassword);
        public string SandboxVendorName => GetValue(x => x.SandboxVendorName);
    }
}