using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using System;

namespace N3O.Umbraco.Scheduler;

public class JobSignatureProvider : IJobSignatureProvider {
    public string GetSignature() {
        var host = Environment.MachineName;
        var version = EnvironmentData.GetOurValue(EnvironmentVariables.Version);

        if (!host.HasValue() && !version.HasValue()) {
            return null;
        }

        return $"{host}:{version}";
    }
}
