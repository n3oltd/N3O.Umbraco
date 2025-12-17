using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Metadata;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms;

public class PlatformsCampaignPageMetadataProvider : PlatformsMetadataProvider {
    public PlatformsCampaignPageMetadataProvider(IPlatformsPageAccessor platformsPageAccessor) 
        : base(platformsPageAccessor) { }
    
    protected override PublishedFileKind Kind => PublishedFileKinds.CampaignPage;
    
    protected override Task<IEnumerable<MetadataEntry>> GetEntriesAsync(PlatformsPage platformsPage) {
        return Task.FromResult<IEnumerable<MetadataEntry>>([]);
    }
}