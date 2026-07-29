using Flurl;
using N3O.Umbraco.Canonical;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms;

public class PlatformsCanonicalUrlProvider : ICanonicalUrlProvider {
    private readonly IPlatformsPageAccessor _platformsPageAccessor;
    private readonly IUrlBuilder _urlBuilder;

    public PlatformsCanonicalUrlProvider(IPlatformsPageAccessor platformsPageAccessor, IUrlBuilder urlBuilder) {
        _platformsPageAccessor = platformsPageAccessor;
        _urlBuilder = urlBuilder;
    }

    public async Task<string> GetUrlAsync(IPublishedContent content) {
        var getPageResult = await _platformsPageAccessor.GetAsync();

        if (!getPageResult.HasValue(x => x.Page?.Url)) {
            return null;
        }

        var rootUrl = _urlBuilder.Root();
        var url = new Url(getPageResult.Page.Url.AbsolutePath);

        url.Scheme = rootUrl.Scheme;
        url.Host = rootUrl.Host;
        url.Port = rootUrl.Port;

        return url;
    }

    public async Task<bool> IsProviderForAsync(IPublishedContent content) {
        var getPageResult = await _platformsPageAccessor.GetAsync();

        return getPageResult.HasValue(x => x.Page);
    }
}
