using N3O.Umbraco.Canonical;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms;

public class PlatformsCanonicalUrlProvider : ICanonicalUrlProvider {
    private readonly IContentCache _contentCache;
    private readonly IPlatformsPageAccessor _platformsPageAccessor;
    private readonly IUrlBuilder _urlBuilder;

    public PlatformsCanonicalUrlProvider(IContentCache contentCache,
                                         IPlatformsPageAccessor platformsPageAccessor,
                                         IUrlBuilder urlBuilder) {
        _contentCache = contentCache;
        _platformsPageAccessor = platformsPageAccessor;
        _urlBuilder = urlBuilder;
    }

    public async Task<string> GetUrlAsync(IPublishedContent content) {
        var getPageResult = await _platformsPageAccessor.GetAsync();

        if (!getPageResult.HasValue(x => x.Page)) {
            return null;
        }

        return getPageResult.Page.AbsoluteUrl(_urlBuilder);
    }

    public bool IsProviderFor(IPublishedContent content) {
        return content.IsPlatformsPage(_contentCache);
    }
}
