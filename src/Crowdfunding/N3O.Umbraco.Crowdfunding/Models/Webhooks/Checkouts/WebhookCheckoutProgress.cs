using N3O.Umbraco.Webhooks.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookCheckoutProgress : Value {
    [JsonConstructor]
    public WebhookCheckoutProgress(IEnumerable<WebhookLookup> requiredStages,
                                   IEnumerable<WebhookLookup> remainingStages) {
        RequiredStages = requiredStages;
        RemainingStages = remainingStages;
    }

    public IEnumerable<WebhookLookup> RequiredStages { get; }
    public IEnumerable<WebhookLookup> RemainingStages { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return RequiredStages;
        yield return RemainingStages;
    }
}