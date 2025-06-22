using N3O.Umbraco.Dev;

namespace N3O.Umbraco.Cloud.Extensions;

public static class DevProfileExtensions {
    public static T SetSubscriptionId<T>(this T devProfile, string dataRegion, string subscriptionCode)
        where T : DevProfile {
        devProfile.SetOurEnvironmentData(CloudConstants.Environment.Keys.DataRegion, dataRegion);
        devProfile.SetOurEnvironmentData(CloudConstants.Environment.Keys.SubscriptionCode, subscriptionCode);

        return devProfile;
    }
}