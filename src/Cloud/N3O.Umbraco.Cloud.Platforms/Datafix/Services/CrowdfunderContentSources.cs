using N3O.Umbraco.Extensions;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms;

// TODO Delete this class along with the legacy crowdfunding composition once every site has completed the
// migration; it only exists to tell the backfill where the legacy content lives and where to put it.
public static class CrowdfunderContentSources {
    private static readonly List<CrowdfunderContentSource> Sources = [];

    public static IReadOnlyList<CrowdfunderContentSource> All => Sources;

    // Called from a site's own composer, once per crowdfunder property it wants populated. The destination alias is
    // the site's own, because the crowdfunder's properties mirror the crowdfunder page schema configured in the
    // backend rather than a set this package defines. Sources are tried in order and the first with content wins.
    public static void Copies(string destinationAlias, params string[] sourceAliases) {
        if (Sources.None(x => x.DestinationAlias == destinationAlias)) {
            Sources.Add(new CrowdfunderContentSource(destinationAlias, sourceAliases));
        }
    }
}
