using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using System;

namespace N3O.Umbraco.Scheduler;

public static class JobSignatureProvider {
    private const char VersionSeparator = ':';

    public static string GetSignature() {
        var host = Environment.MachineName;
        var version = EnvironmentData.GetOurValue(EnvironmentVariables.Version);

        return $"{host}{VersionSeparator}{version}";
    }

    /// <summary>
    /// Whether a job stamped with the given signature should wait for the runtime that queued it rather than run
    /// on this one. True when that runtime is a newer version, because this runtime is then the one being
    /// replaced, or when it is the same version on another host, because either runtime could be the one
    /// leaving. False when it is an older version, since that runtime is being replaced and will not return.
    /// </summary>
    public static bool ShouldDefer(string signature) {
        var currentSignature = GetSignature();

        if (!signature.HasValue() || signature.EqualsInvariant(currentSignature)) {
            return false;
        }

        var scheduledVersion = ParseVersion(signature);
        var currentVersion = ParseVersion(currentSignature);

        if (scheduledVersion.EqualsInvariant(currentVersion)) {
            return true;
        }

        if (Version.TryParse(scheduledVersion, out var scheduled) &&
            Version.TryParse(currentVersion, out var current)) {
            return scheduled > current;
        }

        return false;
    }

    private static string ParseVersion(string signature) {
        var separatorIndex = signature.IndexOf(VersionSeparator);

        if (separatorIndex < 0) {
            return null;
        }

        return signature.Substring(separatorIndex + 1);
    }
}
