using N3O.Umbraco.Logging;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud;

public class SubscriptionLogEnricher : LogEnricher {
    private readonly ISubscriptionAccessor _subscriptionAccessor;

    public SubscriptionLogEnricher(ISubscriptionAccessor subscriptionAccessor) {
        _subscriptionAccessor = subscriptionAccessor;
    }

    public override IReadOnlyDictionary<string, string> GetTags() {
        var data = new Dictionary<string, string>();

        var subscription = _subscriptionAccessor.GetSubscription();

        if (subscription?.Descriptor != null) {
            data["subscriptionId"] = subscription.Descriptor.ToString();
        }

        if (subscription?.DataRegion != null) {
            data["dataRegion"] = subscription.DataRegion.Id;
        }

        return data;
    }
}