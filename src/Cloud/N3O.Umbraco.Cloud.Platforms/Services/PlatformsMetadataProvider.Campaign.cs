using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms;

public class PlatformsCampaignMetadataProvider : PlatformsMetadataProvider {
    public PlatformsCampaignMetadataProvider(IPlatformsPageAccessor platformsPageAccessor) 
        : base(platformsPageAccessor) { }

    protected override Task PopulateMetadataAsync(Dictionary<string, object> metadata, PlatformsPage platformsPage) {
        // Populate additional metadata if needed
        
        return Task.CompletedTask;
    }
    
    protected override PublishedFileKind Kind => PublishedFileKinds.Campaign;
}