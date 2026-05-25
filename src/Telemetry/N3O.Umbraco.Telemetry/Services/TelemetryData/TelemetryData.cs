using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
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

    public string GetVersion() {
        var version = EnvironmentData.GetOurValue(EnvironmentVariables.Version);

        if (!version.HasValue()) {
            version = Assembly.GetEntryAssembly()
                              ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                              ?.InformationalVersion;
        }

        if (version.HasValue() && version.Contains('+')) {
            version = version.Substring(0, version.IndexOf('+'));
        }

        return version;
    }
}
