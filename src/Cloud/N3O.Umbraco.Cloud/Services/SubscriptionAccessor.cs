using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud;

public class SubscriptionAccessor : ISubscriptionAccessor {
    private SubscriptionInfo _subscription;

    public string GetId() {
        return GetSubscription().Id;
    }

    public SubscriptionInfo GetSubscription() {
        if (_subscription == null) {
            var dataRegionId = Get(CloudConstants.Environment.Keys.DataRegion);
            var dataRegion = StaticLookups.FindById<DataRegion>(dataRegionId);
            var subscriptionCode = Get(CloudConstants.Environment.Keys.SubscriptionCode);

            _subscription = new SubscriptionInfo(dataRegion, SubscriptionId.FromCode(subscriptionCode));
        }

        return _subscription;
    }

    private string Get(string setting) {
        return EnvironmentData.GetOurValue(setting);
    }
}