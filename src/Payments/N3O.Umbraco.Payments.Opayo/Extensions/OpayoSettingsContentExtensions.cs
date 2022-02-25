using N3O.Umbraco.Payments.Opayo.Content;
using N3O.Umbraco.Payments.Opayo.Models;

namespace N3O.Umbraco.Payments.Opayo.Extensions {
    public static class OpayoSettingsContentExtensions {
        public static OpayoApiSettings GetProductionSettings(this OpayoSettingsContent settings) {
            var apiSettings = new OpayoApiSettings("https://pi-live.sagepay.com",
                                                   settings.ProductionIntegrationKey,
                                                   settings.ProductionIntegrationPassword,
                                                   settings.ProductionVendorName);
            return apiSettings;
        }

        public static OpayoApiSettings GetSandboxSettings(this OpayoSettingsContent settings) {
            var apiSettings = new OpayoApiSettings("https://pi-test.sagepay.com",
                                                                 settings.StagingIntegrationKey,
                                                                 settings.StagingIntegrationPassword,
                                                                 settings.StagingVendorName);
            return apiSettings;
        }
    }
}