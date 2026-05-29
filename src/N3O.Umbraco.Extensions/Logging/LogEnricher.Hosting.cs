using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using System.Collections.Generic;

namespace N3O.Umbraco.Logging;

public class HostingLogEnricher : LogEnricher {
    public override IReadOnlyDictionary<string, string> GetContextData() => Build();

    public override IReadOnlyDictionary<string, string> GetTags() => Build();

    private static Dictionary<string, string> Build() {
        var data = new Dictionary<string, string>();

        var canonicalDomain = EnvironmentData.GetOurValue(HostingConstants.Environment.Keys.CanonicalDomain);

        if (canonicalDomain.HasValue()) {
            data["canonicalDomain"] = canonicalDomain;
        }

        return data;
    }
}
