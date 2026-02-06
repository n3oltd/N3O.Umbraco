using N3O.Umbraco.Constants;
using N3O.Umbraco.Dev;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Extensions;

public static class DevProfileExtensions {
    public static T SetCanonicalDomain<T>(this T devProfile, string canonicalDomain)
        where T : DevProfile {
        devProfile.SetOurEnvironmentData(HostingConstants.Environment.Keys.CanonicalDomain, canonicalDomain);
        devProfile.SetOurEnvironmentData(HostingConstants.Environment.Keys.CanonicalDomain, canonicalDomain);

        return devProfile;
    }
    
    public static T SetDateFormat<T>(this T devProfile, DateFormat dateFormat) where T : DevProfile {
        devProfile.SetOurEnvironmentData(LocalizationKeys.DateFormat, dateFormat.Id);

        return devProfile;
    }
    
    public static T SetNumberFormat<T>(this T devProfile, NumberFormat numberFormat) where T : DevProfile {
        devProfile.SetOurEnvironmentData(LocalizationKeys.NumberFormat, numberFormat.Id);

        return devProfile;
    }
    
    public static T SetTimeFormat<T>(this T devProfile, TimeFormat timeFormat) where T : DevProfile {
        devProfile.SetOurEnvironmentData(LocalizationKeys.TimeFormat, timeFormat.Id);

        return devProfile;
    }
    
    public static T SetTimezone<T>(this T devProfile, Timezone timezone) where T : DevProfile {
        devProfile.SetOurEnvironmentData(LocalizationKeys.Timezone, timezone.Id);

        return devProfile;
    }
}