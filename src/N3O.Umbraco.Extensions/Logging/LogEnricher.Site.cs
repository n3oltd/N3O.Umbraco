using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System.Collections.Generic;

namespace N3O.Umbraco.Logging;

public class SiteLogEnricher : LogEnricher {
    public override IReadOnlyDictionary<string, string> GetContextData() {
        var contextData = new Dictionary<string, string>();

        if (Site.Language.HasValue()) {
            contextData["SiteLanguage"] = Site.Language;
        }
        
        if (Site.Name.HasValue()) {
            contextData["SiteName"] = Site.Name;
        }
        
        return contextData;
    }
}