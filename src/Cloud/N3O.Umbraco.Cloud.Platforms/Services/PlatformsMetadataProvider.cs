using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms;

public abstract class PlatformsMetadataProvider : IPlatformsMetadataProvider {
    private readonly IPlatformsPageAccessor _platformsPageAccessor;
    
    public PlatformsMetadataProvider(IPlatformsPageAccessor platformsPageAccessor) {
        _platformsPageAccessor = platformsPageAccessor;
    }
    
    public async Task<bool> IsProviderForAsync() {
        var platformsPage = await _platformsPageAccessor.GetAsync();

        return platformsPage?.Kind == Kind;
    }

    public async Task<IReadOnlyDictionary<string, object>> GetAsync() {
        var platformsPage = await _platformsPageAccessor.GetAsync();
        
        var metaData = new Dictionary<string, object>();
        metaData.Add(platformsPage.Kind.MetaTagName, platformsPage.Id);
        
        await PopulateMetadataAsync(metaData, platformsPage);
        
        return metaData;
    }

    protected abstract Task PopulateMetadataAsync(Dictionary<string, object> metadata, PlatformsPage platformsPage);
    protected abstract PublishedFileKind Kind { get; }
}