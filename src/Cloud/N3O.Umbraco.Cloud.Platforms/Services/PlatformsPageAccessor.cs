using Flurl;
using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Content;
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
        var platformsPath = PlatformsPathParser.ParseUri(_contentCache, requestUri);

        if (platformsPath == null) {
            return null;
        }
        
        var currentPath = platformsPath;

        do {
            var (id, kind, mergeModel) = await _cdnClient.DownloadPublishedPageAsync(currentPath);

            if (kind.HasValue()) {
                var platformsPage = new PlatformsPage(id, currentPath, kind, mergeModel);
                var platformsPageUrl = GetPlatformsPageUrl(platformsPage);
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

        var donatePath = PlatformsPathParser.GetDonatePath(_contentCache);

        return new FoundPlatformsPage(null, donatePath);
    }
    
    private string GetPlatformsPageUrl(PlatformsPage platformsPage) {
        var donatePath = PlatformsPathParser.GetDonatePath(_contentCache).Trim('/');
        var platformsPath = platformsPage.Path.Trim('/');

        var url = new Url();
        url.AppendPathSegment(donatePath);
        url.AppendPathSegment(platformsPath);
                    
        return url.ToString();
    }
}