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

    // Defers on any mismatch, in either direction, as a signature identifies the runtime that queued the job
    // and cannot tell an upgrade from a rollback.
    public static bool ShouldDefer(string signature) {
        return signature.HasValue() && !signature.EqualsInvariant(GetSignature());
    }
}
