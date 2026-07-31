using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using System;

namespace N3O.Umbraco.Scheduler;

public static class JobOriginProvider {
    private const char VersionSeparator = ':';

    public static string GetOrigin() {
        var host = Environment.MachineName;
        var version = EnvironmentData.GetOurValue(EnvironmentVariables.Version);

        return $"{host}{VersionSeparator}{version}";
    }

    public static bool ShouldDefer(string origin) {
        return origin.HasValue() && !origin.EqualsInvariant(GetOrigin());
    }
}
