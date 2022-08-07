using Ingenico.Connect.Sdk;
using Ingenico.Connect.Sdk.DefaultImpl;
using Ingenico.Connect.Sdk.Merchant;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Worldline.Content;
using N3O.Umbraco.Payments.Worldline.Models;
using System;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Payments.Worldline;

public class WorldlineComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(WorldlineConstants.ApiName);

        builder.Services.AddSingleton<WorldlineApiSettings>(serviceProvider => {
            var contentCache = serviceProvider.GetRequiredService<IContentCache>();
            var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            var apiSettings = GetApiSettings(contentCache, webHostEnvironment);
            
            return apiSettings;
        });

        builder.Services.AddTransient<MerchantClient>(serviceProvider => {
            var apiSettings = serviceProvider.GetRequiredService<WorldlineApiSettings>();
            MerchantClient client = null;

            if (apiSettings != null) {
                client = Factory.CreateClient(GetClientConfiguration(apiSettings)).Merchant(apiSettings.MerchantId);
            }
            
            return client;
        });
    }
    
    private WorldlineApiSettings GetApiSettings(IContentCache contentCache, IWebHostEnvironment webHostEnvironment) {
        var settings = contentCache.Single<WorldlineSettingsContent>();
        
        if (settings != null) {
            if (webHostEnvironment.IsProduction()) {
                return new WorldlineApiSettings(settings.Platform,
                                                settings.Platform.GetEndpoint(webHostEnvironment),
                                                settings.ProductionApiKey,
                                                settings.ProductionApiSecret,
                                                settings.ProductionMerchantId);
            } else {
                return new WorldlineApiSettings(settings.Platform,
                                                settings.Platform.GetEndpoint(webHostEnvironment),
                                                settings.StagingApiKey,
                                                settings.StagingApiSecret,
                                                settings.StagingMerchantId);
            }
        }

        return null;
    }

    private CommunicatorConfiguration GetClientConfiguration(WorldlineApiSettings apiSettings) {
        var config = new CommunicatorConfiguration();

        config.ConnectTimeout = TimeSpan.FromSeconds(5);
        config.SocketTimeout = TimeSpan.FromSeconds(30);
        config.MaxConnections = 10;
        config.AuthorizationType = AuthorizationType.V1HMAC;
        config.Integrator = "N3O";
        config.ApiEndpoint = apiSettings.Endpoint;
        config.ApiKeyId = apiSettings.ApiKey;
        config.SecretApiKey = apiSettings.ApiSecret;

        return config;
    }
}
