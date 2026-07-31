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
    /// on this one. Any runtime other than the one that queued a job defers it, in either direction, because a
    /// signature cannot tell which of two overlapping runtimes is the one being replaced: an upgrade and a
    /// rollback are indistinguishable. Deferral is bounded, so deferring needlessly costs lateness, whereas
    /// running on a runtime that is about to be replaced loses the work outright.
    /// </summary>
    public static bool ShouldDefer(string signature) {
        return signature.HasValue() && !signature.EqualsInvariant(GetSignature());
    }
}
