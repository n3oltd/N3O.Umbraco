using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Dev;

namespace N3O.Umbraco.Cloud.Extensions;

public static class DevProfileExtensions {
    public static T SetSubscription<T>(this T devProfile, DataRegion dataRegion, SubscriptionId subscriptionId)
        where T : DevProfile {
        devProfile.SetOurEnvironmentData(CloudConstants.Environment.Keys.DataRegion, dataRegion.Id);
        devProfile.SetOurEnvironmentData(CloudConstants.Environment.Keys.SubscriptionId, subscriptionId.Value);

        return devProfile;
    }
}