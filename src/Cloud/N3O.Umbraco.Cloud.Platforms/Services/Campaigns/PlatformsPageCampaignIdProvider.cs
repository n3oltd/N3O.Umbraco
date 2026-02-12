using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms;

[Order(0)]
public class PlatformsPageCampaignIdProvider : ICampaignIdProvider {
    private readonly IPlatformsPageAccessor _platformsPageAccessor;

    public PlatformsPageCampaignIdProvider(IPlatformsPageAccessor platformsPageAccessor) {
        _platformsPageAccessor = platformsPageAccessor;
    }
    
    public async Task<EntityId> GetIdAsync(IPublishedContent content, CancellationToken cancellationToken = default) {
        var getPageResult = await _platformsPageAccessor.GetAsync(cancellationToken);

        if (getPageResult.HasValue(x => x.Page)) {
            return getPageResult.Page.GetCampaignId();
        } else {
            return null;   
        }
    }
}