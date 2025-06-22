using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Hosting;

namespace N3O.Umbraco.Cloud;

public class SubscriptionAccessor : ISubscriptionAccessor {
    private SubscriptionInfo _subscription;

    public string GetId() {
        return GetSubscription().Id;
    }

    public SubscriptionInfo GetSubscription() {
        if (_subscription == null) {
            var dataRegion = Get(CloudConstants.Environment.Keys.DataRegion);
            var subscriptionCode = Get(CloudConstants.Environment.Keys.SubscriptionCode);

            _subscription = new SubscriptionInfo(dataRegion, SubscriptionId.FromCode(subscriptionCode));
        }

        return _subscription;
    }

    private string Get(string setting) {
        return EnvironmentData.GetOurValue(setting);
    }
}