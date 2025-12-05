using Microsoft.AspNetCore.Http;
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
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PlatformsContentFinder(IPlatformsPageAccessor platformsPageAccessor,
                                  IContentCache contentCache,
                                  IHttpContextAccessor httpContextAccessor) {
        _platformsPageAccessor = platformsPageAccessor;
        _contentCache = contentCache;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<bool> TryFindContent(IPublishedRequestBuilder request) {
        var found = false;
        
        var foundPlatformsPage = await _platformsPageAccessor.GetAsync();

        if (foundPlatformsPage.HasValue(x => x.RedirectUrl)) {
            _httpContextAccessor.HttpContext?.Response.Redirect(foundPlatformsPage.RedirectUrl, permanent: true);
        } else if (foundPlatformsPage.HasValue(x => x.Kind)) {
            if (foundPlatformsPage.Kind == PublishedFileKinds.Campaign) {
                request.SetPublishedContent(_contentCache.Special(PlatformsSpecialPages.Campaign));

                found = true;
            } else if (foundPlatformsPage.Kind == PublishedFileKinds.Offering) {
                request.SetPublishedContent(_contentCache.Special(PlatformsSpecialPages.Offering));

                found = true;
            } else {
                // No op
            }
        }
        
        return found;
    }
}