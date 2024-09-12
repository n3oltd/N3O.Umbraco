using N3O.Umbraco.Crm.Engage.Models;
using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Crm.Engage;

public class SubscriptionAccessor : ISubscriptionAccessor {
    private SubscriptionInfo _subscription;

    public string GetId() {
        return Environment.GetEnvironmentVariable($"N3O_SubscriptionId");
    }

    public SubscriptionInfo GetSubscription() {
        if (_subscription == null) {
            var subscriptionId = Get("SubscriptionId").TryParseAs<int>();
            var dataRegion = Get("DataRegion");

            _subscription = new SubscriptionInfo(subscriptionId.Value, dataRegion);
        }

        return _subscription;
    }

    private string Get(string setting) {
        return Environment.GetEnvironmentVariable($"N3O_{setting}");
    }
}