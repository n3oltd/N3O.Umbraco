using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Cloud.Platforms.ContentFinders;

public class PlatformsContentFinder : IContentFinder {
    private readonly IPlatformsPageAccessor _platformsPageAccessor;
    private readonly IContentCache _contentCache;

    public PlatformsContentFinder(IPlatformsPageAccessor platformsPageAccessor, IContentCache contentCache) {
        _platformsPageAccessor = platformsPageAccessor;
        _contentCache = contentCache;
    }
    
    public async Task<bool> TryFindContent(IPublishedRequestBuilder request) {
        var found = false;
        var platformsPage = await _platformsPageAccessor.GetAsync();

        if (platformsPage.HasValue()) {
            if (platformsPage.Kind == PublishedFileKinds.Campaign) {
                request.SetPublishedContent(_contentCache.Special(PlatformsSpecialPages.Campaign));

                found = true;
            } else if (platformsPage.Kind == PublishedFileKinds.Designation) {
                request.SetPublishedContent(_contentCache.Special(PlatformsSpecialPages.Designation));

                found = true;
            }
        }
        
        return found;
    }
}