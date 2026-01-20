using N3O.Umbraco.Extensions;
using N3O.Umbraco.Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms;

public class PlatformsPageMetadataProvider : IMetadataProvider {
    private readonly IPlatformsPageAccessor _platformsPageAccessor;
    
    public PlatformsPageMetadataProvider(IPlatformsPageAccessor platformsPageAccessor) {
        _platformsPageAccessor = platformsPageAccessor;
    }
    
    public Task<bool> IsProviderForAsync(IPublishedContent _) {
        return Task.FromResult(true);
    }
    
    public async Task<IEnumerable<MetadataEntry>> GetEntriesAsync(IPublishedContent _) {
        var getPageResult = await _platformsPageAccessor.GetAsync();
        
        var entries = new List<MetadataEntry>();

        if (getPageResult.HasAny(x => x.Page?.MetaTags)) {
            entries.AddRange(getPageResult.Page.MetaTags.Select(x => new MetadataEntry(x.Key, x.Value)));
        }

        return entries;
    }
}