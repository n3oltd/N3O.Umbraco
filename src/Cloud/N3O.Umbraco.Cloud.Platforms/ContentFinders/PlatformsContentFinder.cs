using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

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
        
        var requestUri = _httpContextAccessor.HttpContext?.Request.Uri();
        var platformsPage = await _platformsPageAccessor.GetAsync();

        if (platformsPage.HasValue()) {
            if (platformsPage.Kind == PublishedFileKinds.Campaign) {
                request.SetPublishedContent(_contentCache.Special(PlatformsSpecialPages.Campaign));

                found = true;
            } else if (platformsPage.Kind == PublishedFileKinds.Designation) {
                request.SetPublishedContent(_contentCache.Special(PlatformsSpecialPages.Designation));

                found = true;
            }
        } else if (ShouldRedirectToDonatePage(requestUri)) {
            var donatePage = _contentCache.Special(SpecialPages.Donate);
            
            request.SetPublishedContent(_contentCache.Special(SpecialPages.Donate));
            
            _httpContextAccessor.HttpContext?.Response.Redirect(donatePage.RelativeUrl(), permanent: true);

            found = true;
        }
        
        return found;
    }

    private bool ShouldRedirectToDonatePage(Uri requestUri) {
        var requestedPath = requestUri.GetAbsolutePathDecoded().ToLowerInvariant().StripTrailingSlash();
        var donatePath = _contentCache.GetDonatePath();

        return requestedPath.StartsWith(donatePath) && !donatePath.EqualsInvariant(requestedPath);
    }
}