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

    public static bool IsConcurrentRuntime(string signature) {
        var currentSignature = GetSignature();

        if (!signature.HasValue() || signature.EqualsInvariant(currentSignature)) {
            return false;
        }

        return ParseVersion(signature).EqualsInvariant(ParseVersion(currentSignature));
    }

    private static string ParseVersion(string signature) {
        var separatorIndex = signature.IndexOf(VersionSeparator);

        if (separatorIndex < 0) {
            return null;
        }

        return signature.Substring(separatorIndex + 1);
    }
}
