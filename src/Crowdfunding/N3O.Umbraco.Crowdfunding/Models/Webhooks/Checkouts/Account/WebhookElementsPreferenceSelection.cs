using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Lookups;
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

    public ConsentChoice ToConsentChoice(ILookups lookups) {
        var channel = lookups.FindById<ConsentChannel>(Channel.Id);
        var category = lookups.FindById<ConsentCategory>(Category.Id);
        var response = lookups.FindById<ConsentResponse>(Response.Id);
            
        return new ConsentChoice(channel, category, response);
    }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Channel;
        yield return Category;
        yield return Response;
    }
}
