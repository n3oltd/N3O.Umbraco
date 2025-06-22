using N3O.Umbraco.Dev;

namespace N3O.Umbraco.Cms.Extensions;

public static class DevProfileExtensions {
    public static T SetCanonicalDomain<T>(this T devProfile, string canonicalDomain)
        where T : DevProfile {
        devProfile.SetOurEnvironmentData(CmsConstants.Environment.Keys.CanonicalDomain, canonicalDomain);
        devProfile.SetOurEnvironmentData(CmsConstants.Environment.Keys.CanonicalDomain, canonicalDomain);

        return devProfile;
    }
}