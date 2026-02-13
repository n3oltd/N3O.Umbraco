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
        
        var getPageResult = await _platformsPageAccessor.GetAsync();

        if (getPageResult.HasValue(x => x.Redirect)) {
            _httpContextAccessor.HttpContext?.Response.Redirect(getPageResult.Redirect.UrlOrPath,
                                                                permanent: !getPageResult.Redirect.Temporary);
        } else if (getPageResult.HasValue(x => x.Page)) {
            if (getPageResult.Page.Kind == PublishedFileKinds.CampaignPage) {
                request.SetPublishedContent(_contentCache.Special(PlatformsSpecialPages.Campaign));

                found = true;
            } else if (getPageResult.Page.Kind == PublishedFileKinds.CrowdfunderPage) {
                request.SetPublishedContent(_contentCache.Special(PlatformsSpecialPages.Crowdfunder));

                found = true;
            }
            else if (getPageResult.Page.Kind == PublishedFileKinds.OfferingPage) {
                request.SetPublishedContent(_contentCache.Special(PlatformsSpecialPages.Offering));

                found = true;
            } else {
                // No op
            }
        }
        
        return found;
    }
}