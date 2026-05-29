using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System.Collections.Generic;

namespace N3O.Umbraco.Logging;

public class SiteLogEnricher : LogEnricher {
    public override IReadOnlyDictionary<string, string> GetTags() {
        var data = new Dictionary<string, string>();

        if (Site.Id.HasValue()) {
            data["siteId"] = Site.Id;
        }

        if (Site.Language.HasValue()) {
            data["siteLanguage"] = Site.Language;
        }

        return data;
    }
}