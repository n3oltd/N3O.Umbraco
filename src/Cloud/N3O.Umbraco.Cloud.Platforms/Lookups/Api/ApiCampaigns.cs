using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

[Order(int.MaxValue)]
public class ApiCampaigns : ApiLookupsCollection<Campaign> {
    private readonly ICdnClient _cdnClient;

    public ApiCampaigns(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }
    
    protected override async Task<IReadOnlyList<Campaign>> FetchAsync(CancellationToken cancellationToken) {
        var publishedCampaigns = await _cdnClient.DownloadSubscriptionContentAsync<PublishedCampaigns>(SubscriptionFiles.Campaigns,
                                                                                                       JsonSerializers.JsonProvider,
                                                                                                       cancellationToken);

        var campaigns = new List<Campaign>();

        foreach (var publishedCampaign in publishedCampaigns.Campaigns) {
            var campaign = new Campaign(publishedCampaign.Id, publishedCampaign.Name, null);
            
            campaigns.Add(campaign);
        }

        return campaigns;
    }

    protected override TimeSpan ReloadInterval => TimeSpan.FromMinutes(1);
}  