using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Lookups;

[Order(int.MaxValue)]
public class ApiSponsorshipSchemes : ApiLookupsCollection<SponsorshipScheme> {
    private readonly ICdnClient _cdnClient;

    public ApiSponsorshipSchemes(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }
    
    protected override async Task<IReadOnlyList<SponsorshipScheme>> FetchAsync(CancellationToken cancellationToken) {
        var publishedSponsorshipSchemes =
            await _cdnClient.DownloadSubscriptionContentAsync<PublishedSponsorshipSchemes>(SubscriptionFiles.SponsorshipSchemes,
                                                                                           JsonSerializers.JsonProvider,
                                                                                           cancellationToken);

        var sponsorshipSchemes = new List<SponsorshipScheme>();

        foreach (var publishedSponsorshipScheme in publishedSponsorshipSchemes.SponsorshipSchemes) {
            var sponsorshipScheme = new SponsorshipScheme(publishedSponsorshipScheme.Id,
                                                          publishedSponsorshipScheme.Name,
                                                          null,
                                                          publishedSponsorshipScheme.AllowedGivingTypes,
                                                          publishedSponsorshipScheme.AllowedDurations.Select(x => StaticLookups.FindById<SponsorshipDuration>(x.Id)).ExceptNull(),
                                                          publishedSponsorshipScheme.FundDimensionOptions.IfNotNull(x => new FundDimensionOptions(x)),
                                                          publishedSponsorshipScheme.Components.Select(x => x.GetSponsorshipComponent(publishedSponsorshipScheme.Id)),
                                                          publishedSponsorshipScheme.AvailableLocations);
            
            sponsorshipSchemes.Add(sponsorshipScheme);
        }

        return sponsorshipSchemes;
    }

    protected override TimeSpan CacheDuration => TimeSpan.FromHours(12);
}
