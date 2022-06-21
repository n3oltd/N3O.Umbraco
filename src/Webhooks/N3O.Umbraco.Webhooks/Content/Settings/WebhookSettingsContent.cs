using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Webhooks.Content;

public class WebhookSettingsContent : UmbracoContent<WebhookSettingsContent> {
    [UmbracoProperty("productionWebhooks")]
    public IEnumerable<WebhookElement> Production => GetNestedAs(x => x.Production);
    
    [UmbracoProperty("stagingWebhooks")]
    public IEnumerable<WebhookElement> Staging => GetNestedAs(x => x.Staging);
}
