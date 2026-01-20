using Flurl;
using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.ContentFinders;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms;

public class PlatformsPageAccessor : IPlatformsPageAccessor {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IContentCache _contentCache;
    private readonly ICdnClient _cdnClient;

    public PlatformsPageAccessor(IHttpContextAccessor httpContextAccessor,
                                 IContentCache contentCache,
                                 ICdnClient cdnClient) {
        _httpContextAccessor = httpContextAccessor;
        _contentCache = contentCache;
        _cdnClient = cdnClient;
    }
    
    public async Task<FoundPlatformsPage> GetAsync() {
        var requestUri = _httpContextAccessor.HttpContext?.Request.Uri();

        foreach (var platformsPageRoute in PlatformsPageRoute.All) {
            var platformsPath = SpecialContentPathParser.ParseUri(_contentCache, platformsPageRoute.Parent, requestUri);

            if (!platformsPath.HasValue()) {
                continue;
            }
        
            var currentPath = platformsPath;

            do {
                var (id, kind, mergeModel) = await _cdnClient.DownloadPublishedPageAsync(platformsPageRoute.ContentKind,
                                                                                         currentPath);

                if (kind.HasValue()) {
                    var platformsPage = new PlatformsPage(id, currentPath, kind, mergeModel);
                    var platformsPageUrl = GetPlatformsPageUrl(platformsPage, platformsPageRoute.Parent);
                    var redirectUrl = currentPath != platformsPath ? platformsPageUrl : null; 
                
                    return new FoundPlatformsPage(platformsPage, redirectUrl);
                }
            
                var lastPathSegment = currentPath.StripTrailingSlash().LastIndexOf('/');
            
                if (lastPathSegment > 0) {
                    currentPath = currentPath.Substring(0, lastPathSegment);
                } else {
                    break;
                }
            } while (currentPath.HasValue());

            return new FoundPlatformsPage(null,
                                          SpecialContentPathParser.GetPath(_contentCache, platformsPageRoute.Parent));
        }
        
        return null;
    }
    
    private string GetPlatformsPageUrl(PlatformsPage platformsPage, SpecialContent parent) {
        var pagePath = SpecialContentPathParser.GetPath(_contentCache, parent).Trim('/');
        var platformsPath = platformsPage.Path.Trim('/');

        var url = new Url();
        url.AppendPathSegment(pagePath);
        url.AppendPathSegment(platformsPath);
                    
        return url.ToString();
    }
}