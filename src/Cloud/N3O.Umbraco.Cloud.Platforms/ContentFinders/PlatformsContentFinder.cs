using Flurl;
using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.IO;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
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
        
        var requestUri = _httpContextAccessor.HttpContext?.Request.Uri();
        
        var isPlatformsDonatePage = PlatformsPathParser.IsPlatformsDonatePage(_contentCache, requestUri);

        if (isPlatformsDonatePage) {
            var (platformsPage, isFallback) = await _platformsPageAccessor.GetAsync();

            if (platformsPage.HasValue()) {
                if (platformsPage.Kind == PublishedFileKinds.Campaign) {
                    request.SetPublishedContent(_contentCache.Special(PlatformsSpecialPages.Campaign));

                    found = true;
                } else if (platformsPage.Kind == PublishedFileKinds.Designation) {
                    request.SetPublishedContent(_contentCache.Special(PlatformsSpecialPages.Designation));

                    found = true;
                }

                if (isFallback) {
                    var fallbackUrl = GetFallbackUrl(requestUri, platformsPage);
                    
                    _httpContextAccessor.HttpContext?.Response.Redirect(fallbackUrl, permanent: true);
                }
            } else {
                var donatePage = _contentCache.Special(SpecialPages.Donate);
            
                request.SetPublishedContent(_contentCache.Special(SpecialPages.Donate));
            
                _httpContextAccessor.HttpContext?.Response.Redirect(donatePage.RelativeUrl(), permanent: true);

                found = true;
            }
        }
        
        return found;
    }

    private string GetFallbackUrl(Uri requestUri, PlatformsPage platformsPage) {
        var donatePath = PlatformsPathParser.GetDonatePath(_contentCache).Trim('/');
        var fallbackPath = platformsPage.Path.Trim('/');
        var baseUrl = Url.Parse(requestUri.AbsoluteUri).Root;

        var url = new Url(baseUrl);
        url.AppendPathSegment(donatePath);
        url.AppendPathSegment(fallbackPath);
                    
        return url.ToString();
    }
}