using N3O.Umbraco.Logging;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud;

public class SubscriptionLogEnricher : LogEnricher {
    private readonly ISubscriptionAccessor _subscriptionAccessor;

    public SubscriptionLogEnricher(ISubscriptionAccessor subscriptionAccessor) {
        _subscriptionAccessor = subscriptionAccessor;
    }

    public override IReadOnlyDictionary<string, string> GetContextData() {
        var contextData = new Dictionary<string, string>();

        var subscription = _subscriptionAccessor.GetSubscription();

        contextData["SubscriptionId"] = subscription.Id.ToString();
        
        return contextData;
    }
}