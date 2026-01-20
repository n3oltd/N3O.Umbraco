using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Metadata;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms;

public class PlatformsOfferingPageMetadataProvider : PlatformsMetadataProvider {
    public PlatformsOfferingPageMetadataProvider(IPlatformsPageAccessor platformsPageAccessor) 
        : base(platformsPageAccessor) { }
    
    protected override PublishedFileKind Kind => PublishedFileKinds.OfferingPage;
    
    protected override Task<IEnumerable<MetadataEntry>> GetEntriesAsync(PlatformsPage platformsPage) {
        // TODO This and the other metadata providers need to extract the campaign ID + other platforms properties
        
        return Task.FromResult<IEnumerable<MetadataEntry>>([]);
    }
}