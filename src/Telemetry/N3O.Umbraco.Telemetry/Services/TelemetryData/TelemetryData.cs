using N3O.Umbraco.Constants;
using N3O.Umbraco.Hosting;
using System.Diagnostics;
using System.Reflection;

namespace N3O.Umbraco.Telemetry;

public class TelemetryData : ITelemetryData {
    public string GetDeploymentVersion() {
        var deploymentVersion = EnvironmentData.GetOurValue(EnvironmentVariables.DeploymentVersion);

        return deploymentVersion;
    }
    
    public string GetExtensionsVersion() {
        var productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
        
        var extensionsVersion = productVersion.Substring(0, productVersion.IndexOf('+'));

        return extensionsVersion;
    }
}
