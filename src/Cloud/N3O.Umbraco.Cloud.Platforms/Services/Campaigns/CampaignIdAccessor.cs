using Humanizer;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
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

        if (getPageResult.Page.HasValue()) {
            if (getPageResult.Page.Kind == PublishedFileKinds.CampaignPage) {
                return getPageResult.Page.Content[nameof(PublishedCampaignPage.Campaign).Camelize()][nameof(PublishedCampaignPage.Campaign.Id).Camelize()].ToString();
            } else if (getPageResult.Page.Kind == PublishedFileKinds.OfferingPage) {
                return getPageResult.Page.Content[nameof(PublishedOfferingPage.Offering).Camelize()][nameof(PublishedOfferingPage.Offering.CampaignId).Camelize()].ToString();
            } else if (getPageResult.Page.Kind == PublishedFileKinds.CrowdfunderPage) {
                return getPageResult.Page.Content[nameof(PublishedCrowdfunderPage.Crowdfunder).Camelize()][nameof(PublishedCrowdfunderPage.Crowdfunder.CampaignId).Camelize()].ToString();
            }
        }

        return null;
    }
}
