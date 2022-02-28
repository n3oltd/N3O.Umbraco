using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Payments.Opayo.Content;
using N3O.Umbraco.Payments.Opayo.Models;

namespace N3O.Umbraco.Payments.Opayo.Extensions {
    public static class ContentCacheExtensions {
        public static OpayoApiSettings GetOpayoApiSettings(this IContentCache contentCache,
                                                           IHostEnvironment environment) {
            var settings = contentCache.Single<OpayoSettingsContent>();
            if (settings != null) {
                if (environment.IsProduction()) {
                    return new OpayoApiSettings("https://pi-live.sagepay.com",
                                                settings.ProductionIntegrationKey,
                                                settings.ProductionIntegrationPassword,
                                                settings.ProductionVendorName);
                }

                return new OpayoApiSettings("https://pi-test.sagepay.com",
                                            settings.StagingIntegrationKey,
                                            settings.StagingIntegrationPassword,
                                            settings.StagingVendorName); 
            }

            return null;
        }
    }
}