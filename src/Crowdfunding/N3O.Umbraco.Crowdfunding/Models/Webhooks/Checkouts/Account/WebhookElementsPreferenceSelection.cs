using N3O.Umbraco.Webhooks.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookElementsPreferenceSelection : Value {
    [JsonConstructor]
    public WebhookElementsPreferenceSelection(WebhookLookup channel, WebhookLookup category, WebhookLookup response) {
        Channel = channel;
        Category = category;
        Response = response;
    }
    
    public WebhookLookup Channel { get; }
    public WebhookLookup Category { get; }
    public WebhookLookup Response { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Channel;
        yield return Category;
        yield return Response;
    }
}
