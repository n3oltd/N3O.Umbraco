using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Metadata;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms;

public abstract class PlatformsMetadataProvider : IMetadataProvider {
    private readonly IPlatformsPageAccessor _platformsPageAccessor;
    
    protected PlatformsMetadataProvider(IPlatformsPageAccessor platformsPageAccessor) {
        _platformsPageAccessor = platformsPageAccessor;
    }
    
    public async Task<bool> IsProviderForAsync(IPublishedContent _) {
        var foundPlatformsPage = await _platformsPageAccessor.GetAsync();

        return foundPlatformsPage?.Kind == Kind;
    }
    
    public async Task<IEnumerable<MetadataEntry>> GetEntriesAsync(IPublishedContent _) {
        var foundPlatformsPage = await _platformsPageAccessor.GetAsync();
        
        var entries = new List<MetadataEntry>();
        
        entries.Add(new MetadataEntry(foundPlatformsPage.Kind.MetaTagName, foundPlatformsPage.Id.ToString("D")));
        
        entries.AddRange(await GetEntriesAsync(foundPlatformsPage));

        return entries;
    }

    protected abstract PublishedFileKind Kind { get; }
    
    protected abstract Task<IEnumerable<MetadataEntry>> GetEntriesAsync(PlatformsPage platformsPage);
}