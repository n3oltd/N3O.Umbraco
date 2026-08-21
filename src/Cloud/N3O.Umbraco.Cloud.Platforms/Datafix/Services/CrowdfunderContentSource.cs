using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms;

// TODO Delete along with the rest of the Datafix folder once every site has completed the migration.
public class CrowdfunderContentSource {
    public CrowdfunderContentSource(string destinationAlias, IReadOnlyList<string> sourceAliases) {
        DestinationAlias = destinationAlias;
        SourceAliases = sourceAliases;
    }

    public string DestinationAlias { get; }
    public IReadOnlyList<string> SourceAliases { get; }
}
