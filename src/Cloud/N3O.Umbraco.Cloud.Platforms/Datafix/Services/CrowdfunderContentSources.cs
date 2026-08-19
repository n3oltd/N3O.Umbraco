using N3O.Umbraco.Extensions;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms;

// TODO Delete along with the rest of the Datafix folder once every site has completed the migration.
public static class CrowdfunderContentSources {
    private static readonly List<CrowdfunderContentSource> Sources = [];

    public static IReadOnlyList<CrowdfunderContentSource> All => Sources;

    // Called from a site's composer. Sources are tried in order and the first with content wins.
    public static void Copies(string destinationAlias, params string[] sourceAliases) {
        if (Sources.None(x => x.DestinationAlias == destinationAlias)) {
            Sources.Add(new CrowdfunderContentSource(destinationAlias, sourceAliases));
        }
    }
}
