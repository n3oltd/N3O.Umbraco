using N3O.Umbraco.Crm.Engage.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;

namespace N3O.Umbraco.Crm.Engage;

public class SubscriptionAccessor : ISubscriptionAccessor {
    private SubscriptionInfo _subscription;

    public string GetId() {
        return GetSubscription().Id;
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
        return EnvironmentSettings.GetValue($"N3O_{setting}");
    }
}