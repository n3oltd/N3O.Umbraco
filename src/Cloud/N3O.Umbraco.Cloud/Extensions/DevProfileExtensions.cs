using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Dev;

namespace N3O.Umbraco.Cloud.Extensions;

public static class DevProfileExtensions {
    public static T SetSubscriptionId<T>(this T devProfile, DataRegion dataRegion, string subscriptionCode)
        where T : DevProfile {
        devProfile.SetOurEnvironmentData(CloudConstants.Environment.Keys.DataRegion, dataRegion.Id);
        devProfile.SetOurEnvironmentData(CloudConstants.Environment.Keys.SubscriptionCode, subscriptionCode);

        return devProfile;
    }
}