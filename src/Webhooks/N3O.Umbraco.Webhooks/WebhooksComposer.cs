using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Webhooks.Attributes;
using N3O.Umbraco.Webhooks.Receivers;
using N3O.Umbraco.Webhooks.Transforms;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Webhooks;

public class WebhooksComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(WebhooksConstants.ApiName);
        
        builder.Services.AddSingleton<IWebhooks, Webhooks>();

        RegisterAll(t => t.ImplementsInterface<IWebhookReceiver>() && t.HasAttribute<WebhookReceiverAttribute>(),
                    t => builder.Services.AddTransient(t));
        
        RegisterAll(t => t.ImplementsInterface<IWebhookTransform>(),
                    t => builder.Services.AddTransient(typeof(IWebhookTransform), t));
    }
}
