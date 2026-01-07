using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

[Order(int.MaxValue)]
public class ApiOfferings : ApiLookupsCollection<Offering> {
    private readonly ICdnClient _cdnClient;

    public ApiOfferings(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }
    
    protected override async Task<IReadOnlyList<Offering>> FetchAsync(CancellationToken cancellationToken) {
        var publishedCampaigns = await _cdnClient.DownloadSubscriptionContentAsync<PublishedCampaigns>(SubscriptionFiles.Campaigns,
                                                                                                       JsonSerializers.JsonProvider,
                                                                                                       cancellationToken);

        var publishedOfferings = publishedCampaigns.Campaigns.SelectMany(x => x.Offerings).ToList();
        
        var offerings = new List<Offering>();

        foreach (var publishedOffering in publishedOfferings) {
            var offering = new Offering(publishedOffering.Id, publishedOffering.Name, null);
            
            offerings.Add(offering);
        }

        return offerings;
    }

    protected override TimeSpan CacheDuration => TimeSpan.FromHours(12);
}