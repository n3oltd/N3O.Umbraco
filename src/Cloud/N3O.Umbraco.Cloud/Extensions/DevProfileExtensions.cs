using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Dev;
using System;

namespace N3O.Umbraco.Cloud.Extensions;

public static class DevProfileExtensions {
    public static T SetSubscription<T>(this T devProfile, DataRegion dataRegion, SubscriptionDescriptor descriptor)
        where T : DevProfile {
        devProfile.SetOurEnvironmentData(CloudConstants.Environment.Keys.DataRegion, dataRegion.Id);
        devProfile.SetOurEnvironmentData(CloudConstants.Environment.Keys.SubscriptionId, descriptor.Id);

        return devProfile;
    }

    [Obsolete("Use SetSubscription with a SubscriptionDescriptor; this shim exists for the existing Dev/Profiles in consuming sites.")]
    public static T SetSubscriptionId<T>(this T devProfile, DataRegion dataRegion, string subscriptionCode)
        where T : DevProfile {
        return SetSubscription(devProfile, dataRegion, SubscriptionDescriptor.FromCode(subscriptionCode));
    }
}