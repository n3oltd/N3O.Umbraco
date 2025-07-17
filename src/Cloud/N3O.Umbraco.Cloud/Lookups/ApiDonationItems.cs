using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Lookups;

[Order(int.MaxValue)]
public class ApiDonationItems : ApiLookupsCollection<DonationItem> {
    private readonly ICdnClient _cdnClient;

    public ApiDonationItems(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }
    
    protected override async Task<IReadOnlyList<DonationItem>> FetchAsync(CancellationToken cancellationToken) {
        var publishedDonationItems = await _cdnClient.DownloadSubscriptionContentAsync<PublishedDonationItems>(SubscriptionFiles.DonationItems,
                                                                                                               JsonSerializers.JsonProvider,
                                                                                                               cancellationToken);

        var donationItems = new List<DonationItem>();

        foreach (var publishedDonationItem in publishedDonationItems.DonationItems) {
            var donationItem = new DonationItem(publishedDonationItem.Id,
                                                publishedDonationItem.Name,
                                                null,
                                                publishedDonationItem.AllowedGivingTypes,
                                                publishedDonationItem.FundDimensionOptions.IfNotNull(x => new FundDimensionOptions(x)),
                                                publishedDonationItem.Pricing.IfNotNull(x => new Pricing(x)));
            
            donationItems.Add(donationItem);
        }

        return donationItems;
    }

    protected override TimeSpan CacheDuration => TimeSpan.FromHours(12);
}
