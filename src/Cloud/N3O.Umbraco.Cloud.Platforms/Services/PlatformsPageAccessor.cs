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
    private PlatformsPage _platformsPage;

    public PlatformsPageAccessor(IHttpContextAccessor httpContextAccessor,
                                 IContentCache contentCache,
                                 ICdnClient cdnClient) {
        _httpContextAccessor = httpContextAccessor;
        _contentCache = contentCache;
        _cdnClient = cdnClient;
    }
    
    public async Task<PlatformsPage> GetAsync() {
        _platformsPage ??= await GetPlatformsPageAsync();

        return _platformsPage;
    }

    private async Task<PlatformsPage> GetPlatformsPageAsync() {
        var requestUri = _httpContextAccessor.HttpContext?.Request.Uri();
        var platformsPath = PlatformsPathParser.ParseUri(_contentCache, requestUri);

        if (platformsPath.HasValue()) {
            var (kind, mergeModel) = await _cdnClient.DownloadPublishedContentAsync(platformsPath);

            if (kind.HasValue()) {
                return new PlatformsPage(kind, mergeModel);
            }
        }
        
        return null;
    }
}