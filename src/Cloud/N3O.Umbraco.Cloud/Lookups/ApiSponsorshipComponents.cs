using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Lookups;

[Order(int.MaxValue)]
public class ApiSponsorshipComponents : ApiLookupsCollection<SponsorshipComponent> {
    private readonly ICdnClient _cdnClient;

    public ApiSponsorshipComponents(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }
    
    protected override async Task<IReadOnlyList<SponsorshipComponent>> FetchAsync(CancellationToken cancellationToken) {
        var publishedSponsorshipSchemes =
            await _cdnClient.DownloadSubscriptionContentAsync<PublishedSponsorshipSchemes>(SubscriptionFiles.SponsorshipSchemes,
                                                                                           JsonSerializers.JsonProvider,
                                                                                           cancellationToken);

        var sponsorshipComponents = new List<SponsorshipComponent>();

        foreach (var publishedSponsorshipScheme in publishedSponsorshipSchemes.SponsorshipSchemes) {
            var schemeSponsorshipComponents =
                publishedSponsorshipScheme.Components.Select(x => x.GetSponsorshipComponent(publishedSponsorshipScheme.Id)).ToList();
            
            sponsorshipComponents.AddRange(schemeSponsorshipComponents);
        }

        return sponsorshipComponents;
    }

    protected override TimeSpan CacheDuration => TimeSpan.FromHours(12);
}
