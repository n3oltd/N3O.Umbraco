using N3O.Umbraco.Constants;
using N3O.Umbraco.Hosting;

namespace N3O.Umbraco.Utilities;

public static class Site {
    public static string Language => EnvironmentData.GetOurValue(EnvironmentVariables.SiteLanguage);
    public static string Name => EnvironmentData.GetOurValue(EnvironmentVariables.SiteName);
}