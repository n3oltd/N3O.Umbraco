using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Templates;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Templates;

public class CampaignsMergeModelProvider : MergeModelsProvider {
    private readonly ICdnClient _cdnClient;

    public CampaignsMergeModelProvider(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }

    protected override async Task PopulateModelsAsync(IPublishedContent content,
                                                      Dictionary<string, object> mergeModels,
                                                      CancellationToken cancellationToken = default) {
        var campaigns = await _cdnClient.DownloadSubscriptionContentAsync<PublishedCampaigns>(SubscriptionFiles.Campaigns,
                                                                                              JsonSerializers.JsonProvider,
                                                                                              cancellationToken);

        mergeModels[PlatformsTemplateConstants.ModelKeys.Campaigns] = campaigns;
    }
}