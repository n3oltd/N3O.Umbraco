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
    private readonly ILookups _lookups;

    public ApiOfferings(ICdnClient cdnClient, ILookups lookups) {
        _cdnClient = cdnClient;
        _lookups = lookups;
    }
    
    protected override async Task<IReadOnlyList<Offering>> FetchAsync(CancellationToken cancellationToken) {
        var publishedCampaigns = await _cdnClient.DownloadSubscriptionContentAsync<PublishedCampaigns>(SubscriptionFiles.Campaigns,
                                                                                                       JsonSerializers.JsonProvider,
                                                                                                       cancellationToken);
        
        var offerings = new List<Offering>();

        foreach (var publishedCampaign in publishedCampaigns.Campaigns) {
            var campaign = _lookups.FindById<Campaign>(publishedCampaign.Id);
            
            var offeringLookups = publishedCampaign.Offerings
                                                   .Select(x => new Offering(x.Id, x.Name, null, campaign));
            
            offerings.AddRange(offeringLookups);
        }

        return offerings;
    }

    protected override TimeSpan CacheDuration => TimeSpan.FromHours(12);
}