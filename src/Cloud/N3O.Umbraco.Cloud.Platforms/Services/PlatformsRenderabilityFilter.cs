using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms;

public class PlatformsRenderabilityFilter : IContentRenderabilityFilter {
    private readonly IContentCache _contentCache;
    private readonly IPlatformsPageAccessor _platformsPageAccessor;

    public PlatformsRenderabilityFilter(IContentCache contentCache, IPlatformsPageAccessor platformsPageAccessor) {
        _contentCache = contentCache;
        _platformsPageAccessor = platformsPageAccessor;
    }
    
    public bool IsFilterFor(IPublishedContent content) {
        if (content == _contentCache.Special(PlatformsSpecialPages.Campaign)) {
            return true;
        } else if (content == _contentCache.Special(PlatformsSpecialPages.Offering)) {
            return true;
        } else {
            return false;
        }
    }
    
    public async Task<bool> CanRenderAsync(IPublishedContent content) {
        var getPageResult = await _platformsPageAccessor.GetAsync();

        return getPageResult?.IsRedirect == false;
    }
}