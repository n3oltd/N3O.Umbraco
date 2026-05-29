using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using System.Collections.Generic;

namespace N3O.Umbraco.Logging;

public class HostingLogEnricher : LogEnricher {
    public override IReadOnlyDictionary<string, string> GetTags() {
        var data = new Dictionary<string, string>();

        var canonicalDomain = EnvironmentData.GetOurValue(HostingConstants.Environment.Keys.CanonicalDomain);

        if (canonicalDomain.HasValue()) {
            data["domain"] = canonicalDomain;
        }

        return data;
    }
}
