using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Webhooks;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Webhooks {
    public class WebhooksComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddSingleton<IWebhooks, Webhooks>();
            
            RegisterAll(t => t.ImplementsInterface<IWebhookTransform>(),
                        t => builder.Services.AddTransient(typeof(IWebhookTransform), t));
        }
    }
}