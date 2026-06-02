using N3O.Umbraco.Extensions;
using N3O.Umbraco.Logging;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud;

public class SubscriptionCodeLogEnricher : LogEnricher {
    private readonly ISubscriptionAccessor _subscriptionAccessor;

    public SubscriptionCodeLogEnricher(ISubscriptionAccessor subscriptionAccessor) {
        _subscriptionAccessor = subscriptionAccessor;
    }

    public override IReadOnlyDictionary<string, string> GetTags() {
        var data = new Dictionary<string, string>();

        var code = _subscriptionAccessor.GetSubscription()?.Descriptor?.Code;

        if (code.HasValue() && code != "0") {
            data["subscriptionCode"] = code;
        }

        return data;
    }
}
