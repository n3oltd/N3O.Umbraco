using N3O.Umbraco.Extensions;
using N3O.Umbraco.Logging;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud;

public class DataRegionLogEnricher : LogEnricher {
    private readonly ISubscriptionAccessor _subscriptionAccessor;

    public DataRegionLogEnricher(ISubscriptionAccessor subscriptionAccessor) {
        _subscriptionAccessor = subscriptionAccessor;
    }

    public override IReadOnlyDictionary<string, string> GetTags() {
        var data = new Dictionary<string, string>();

        var subscription = _subscriptionAccessor.GetSubscription();

        if (subscription?.DataRegion?.Id.HasValue() == true) {
            data["dataRegion"] = subscription.DataRegion.Id;
        }

        return data;
    }
}
