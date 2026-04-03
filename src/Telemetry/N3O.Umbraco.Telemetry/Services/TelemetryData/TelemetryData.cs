using N3O.Umbraco.Constants;
using N3O.Umbraco.Hosting;
using System.Diagnostics;
using System.Reflection;

namespace N3O.Umbraco.Telemetry;

public class TelemetryData : ITelemetryData {
    public string GetExtensionsVersion() {
        var productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
        
        var extensionsVersion = productVersion.Substring(0, productVersion.IndexOf('+'));

        return extensionsVersion;
    }

    public string GetSiteDeploymentVersion() {
        var siteDeploymentVersion = EnvironmentData.GetOurValue(EnvironmentVariables.SiteDeploymentVersion);

        return siteDeploymentVersion;
    }
}