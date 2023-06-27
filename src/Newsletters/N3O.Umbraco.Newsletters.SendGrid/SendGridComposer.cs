using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Json;
using N3O.Umbraco.Newsletters.SendGrid.Content;
using SendGrid;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Newsletters.SendGrid;

public class SendGridComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<IMarketingApiClient>(serviceProvider => {
            var contentCache = serviceProvider.GetRequiredService<IContentCache>();
            var settings = contentCache.Single<SendGridSettingsContent>();
            var jsonProvider = serviceProvider.GetRequiredService<IJsonProvider>();
    
            var sendGridClient = GetSendGridClient(settings.ApiKey);
            var marketingClient = new MarketingApiClient(sendGridClient, jsonProvider);

            return marketingClient;
        });
        
        builder.Services.AddTransient<INewslettersClient, SendGridNewslettersClient>();
    }
    
    private SendGridClient GetSendGridClient(string apiKey) {
        var options = new SendGridClientOptions();
        options.ApiKey = apiKey;
        options.HttpErrorAsException = true;
        
        return new SendGridClient(options);
    }
}