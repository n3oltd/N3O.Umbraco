using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Cdn.Cloudflare.Clients;
using N3O.Umbraco.Cdn.Cloudflare.Content;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Utilities;
using Refit;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Cdn.Cloudflare;

public class CloudflareCdnComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<IRemoteIpAddressAccessor, CloudflareIpAddressAccessor>();

        RegisterStreams(builder);
    }

    private void RegisterStreams(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<IStreamsApiClient>(serviceProvider => {
            var contentCache = serviceProvider.GetRequiredService<IContentCache>();
            var jsonProvider = serviceProvider.GetRequiredService<IJsonProvider>();
            var settings = contentCache.Single<CloudflareSettingsContent>();
            var accountId = settings?.VideoAccountId;
            var token = settings?.VideoToken;
            
            IStreamsApiClient client = null;

            if (accountId.HasValue() && token.HasValue()) {
                var refitSettings = new RefitSettings();
                refitSettings.ContentSerializer = new NewtonsoftJsonContentSerializer(jsonProvider.GetSettings());
                
                refitSettings.HttpMessageHandlerFactory = () => new AddBearerAuthorizationHandler(token);
                
                client = RestService.For<IStreamsApiClient>($"https://api.cloudflare.com/client/v4/accounts/{accountId}/", refitSettings);
            }

            return client;
        });
    }
}
