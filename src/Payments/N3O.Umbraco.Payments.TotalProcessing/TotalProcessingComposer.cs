using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.TotalProcessing.Clients;
using N3O.Umbraco.Payments.TotalProcessing.Content;
using N3O.Umbraco.Payments.TotalProcessing.Models;
using Refit;
using System;
using System.Net.Http;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Payments.TotalProcessing;

public class TotalProcessingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(TotalProcessingConstants.ApiName);
        builder.Services.AddTransient<ITotalProcessingHelper, TotalProcessingHelper>();

        builder.Services.AddSingleton(serviceProvider => {
            var contentCache = serviceProvider.GetRequiredService<IContentCache>();
            var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            var apiSettings = GetApiSettings(contentCache, webHostEnvironment);
            
            return apiSettings;
        });
        
        builder.Services.AddTransient(serviceProvider => {
            var apiSettings = serviceProvider.GetRequiredService<TotalProcessingApiSettings>();

            ITotalProcessingClient client = null;

            if (apiSettings != null) {
                var authorizationHandler = new CheckoutAuthorizationHandler(apiSettings.AccessToken);
                
                var httpClient = new HttpClient(new FormUrlEncodingHandler(authorizationHandler));
                httpClient.BaseAddress = new Uri(apiSettings.BaseUrl);

                var refitSettings = new RefitSettings();
                refitSettings.ContentSerializer = new NewtonsoftJsonContentSerializer();
                
                client = RestService.For<ITotalProcessingClient>(httpClient, refitSettings);
            }
            
            return client;
        });
    }
    
    private TotalProcessingApiSettings GetApiSettings(IContentCache contentCache, IWebHostEnvironment webHostEnvironment) {
        var settings = contentCache.Single<TotalProcessingSettingsContent>();
        
        if (settings != null) {
            if (webHostEnvironment.IsProduction()) {
                return new TotalProcessingApiSettings("https://eu-prod.oppwa.com",
                                                      settings.ProductionAccessToken,
                                                      settings.ProductionEntityId);
            } else {
                return new TotalProcessingApiSettings("https://eu-test.oppwa.com",
                                                      settings.StagingAccessToken,
                                                      settings.StagingEntityId);
            }
        }

        return null;
    }
}
