using N3O.Umbraco.Logging;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud;

public class SubscriptionLogEnricher : LogEnricher {
    private readonly ISubscriptionAccessor _subscriptionAccessor;

    public SubscriptionLogEnricher(ISubscriptionAccessor subscriptionAccessor) {
        _subscriptionAccessor = subscriptionAccessor;
    }

    public override IReadOnlyDictionary<string, string> GetContextData() => Build();

    public override IReadOnlyDictionary<string, string> GetTags() => Build();

    private Dictionary<string, string> Build() {
        var data = new Dictionary<string, string>();

        var subscription = _subscriptionAccessor.GetSubscription();

        if (subscription?.Id != null) {
            data["subscriptionId"] = subscription.Id.ToString();
            data["subscriptionCode"] = subscription.Id.Code;
            data["subscriptionNumber"] = subscription.Id.Number.ToString();
        }

        if (subscription?.DataRegion != null) {
            data["dataRegion"] = subscription.DataRegion.Id;
        }

        return data;
    }
}