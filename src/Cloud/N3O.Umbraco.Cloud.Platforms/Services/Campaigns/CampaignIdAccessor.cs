using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms;

public class CampaignIdAccessor : ICampaignIdAccessor {
    private readonly IPlatformsPageAccessor _platformsPageAccessor;

    public CampaignIdAccessor(IPlatformsPageAccessor platformsPageAccessor) {
        _platformsPageAccessor = platformsPageAccessor;
    }
    
    public async Task<EntityId> GetIdAsync(CancellationToken cancellationToken = default) {
        var getPageResult = await _platformsPageAccessor.GetAsync(cancellationToken);

        if (getPageResult.HasValue(x => x.Page)) {
            return getPageResult.Page.GetCampaignId();
        }

        return null;
    }
}
