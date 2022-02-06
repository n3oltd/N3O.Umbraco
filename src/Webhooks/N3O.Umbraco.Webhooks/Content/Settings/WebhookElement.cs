using N3O.Umbraco.Content;
using N3O.Umbraco.Webhooks.Lookups;

namespace N3O.Umbraco.Webhooks.Content {
    public class WebhookElement : UmbracoElement<WebhookElement> {
        public WebhookEvent Event => GetValue(x => x.Event);
        public string Url => GetValue(x => x.Url);
    }
}