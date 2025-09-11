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
    
    public async Task<(PlatformsPage, bool)> GetAsync() {
        var requestUri = _httpContextAccessor.HttpContext?.Request.Uri();
        var platformsPath = PlatformsPathParser.ParseUri(_contentCache, requestUri);

        var isFallback = false;

        while (platformsPath.HasValue()) {
            var (id, kind, mergeModel) = await _cdnClient.DownloadPublishedPageAsync(platformsPath);

            if (kind.HasValue()) {
                var platformsPage = new PlatformsPage(id, platformsPath, kind, mergeModel);
                
                return (platformsPage, isFallback);
            }

            isFallback = true;
            
            var lastPathSegment = platformsPath.StripTrailingSlash().LastIndexOf('/');
            
            if (lastPathSegment > 0) {
                platformsPath = platformsPath.Substring(0, lastPathSegment);
            } else {
                break;
            }
        }

        return (null, false);
    }
}