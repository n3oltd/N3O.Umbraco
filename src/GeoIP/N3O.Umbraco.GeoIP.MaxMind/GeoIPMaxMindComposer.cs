using MaxMind.GeoIP2;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.GeoIP.MaxMind.Content;
using N3O.Umbraco.GeoIP.MaxMind.Models;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.GeoIP.MaxMind;

public class GeoIPMaxMindComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<IIPGeoLocationProvider, MaxMindIPGeoLocationProvider>();
        
        builder.Services.AddSingleton<MaxMindApiSettings>(serviceProvider => {
            var contentCache = serviceProvider.GetRequiredService<IContentCache>();
            var settingsContent = contentCache.Single<MaxMindSettingsContent>();
            var apiSettings = new MaxMindApiSettings(settingsContent.AccountId, settingsContent.LicenseKey);
            
            return apiSettings;
        });
        
        builder.Services.AddTransient<WebServiceClient>(serviceProvider => {
            var apiSettings = serviceProvider.GetRequiredService<MaxMindApiSettings>();
            WebServiceClient client = null;

            if (apiSettings != null) {
                client = new WebServiceClient(apiSettings.AccountId, apiSettings.LicenseKey);
            }
            
            return client;
        });
    }
}
