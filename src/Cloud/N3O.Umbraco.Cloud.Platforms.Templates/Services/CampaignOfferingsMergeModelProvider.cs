using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Templates;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Templates;

public class CampaignOfferingsMergeModelProvider : MergeModelsProvider {
    private readonly ICampaignIdAccessor _campaignIdAccessor;
    private readonly ICdnClient _cdnClient;

    public CampaignOfferingsMergeModelProvider(ICampaignIdAccessor campaignIdAccessor, ICdnClient cdnClient) {
        _campaignIdAccessor = campaignIdAccessor;
        _cdnClient = cdnClient;
    }

    protected override async Task PopulateModelsAsync(IPublishedContent content,
                                                      Dictionary<string, object> mergeModels,
                                                      CancellationToken cancellationToken = default) {
        var campaignId = await _campaignIdAccessor.GetIdAsync(cancellationToken);

        if (campaignId.HasValue()) {
            var publishedFile = PublishedFileInfo.ForRootFile(PublishedFileKinds.Campaign, campaignId);

            var campaign = await _cdnClient.DownloadPublishedContentAsync<PublishedCampaign>(publishedFile.Kind,
                                                                                             publishedFile.Path,
                                                                                             JsonSerializers.JsonProvider,
                                                                                             cancellationToken);

            mergeModels[PlatformsTemplateConstants.ModelKeys.CampaignOfferings] = campaign.Offerings;
        }
    }
}