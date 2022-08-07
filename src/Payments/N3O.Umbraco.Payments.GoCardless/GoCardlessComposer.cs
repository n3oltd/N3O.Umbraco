using GoCardless;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.GoCardless.Content;
using N3O.Umbraco.Payments.GoCardless.Models;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Payments.GoCardless;

public class GoCardlessComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(GoCardlessConstants.ApiName);
        
        builder.Services.AddSingleton<GoCardlessApiSettings>(serviceProvider => {
            var contentCache = serviceProvider.GetRequiredService<IContentCache>();
            var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            var apiSettings = GetApiSettings(contentCache, webHostEnvironment);
            
            return apiSettings;
        });

        builder.Services.AddTransient<GoCardlessClient>(serviceProvider => {
            var apiSettings = serviceProvider.GetRequiredService<GoCardlessApiSettings>();
            GoCardlessClient client = null;

            if (apiSettings != null) {
                client = GoCardlessClient.Create(apiSettings.AccessToken, apiSettings.Environment);
            }
            
            return client;
        });
    }
    
    public GoCardlessApiSettings GetApiSettings(IContentCache contentCache, IWebHostEnvironment webHostEnvironment) {
        var settings = contentCache.Single<GoCardlessSettingsContent>();
        
        if (settings != null) {
            if (webHostEnvironment.IsProduction()) {
                return new GoCardlessApiSettings(settings.ProductionAccessToken,
                                                 GoCardlessClient.Environment.LIVE);
            } else {
                return new GoCardlessApiSettings(settings.StagingAccessToken,
                                                 GoCardlessClient.Environment.SANDBOX);
            }
        }

        return null;
    }
}
