using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search;
using N3O.Umbraco.Search.Models;
using N3O.Umbraco.Utilities;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms.Search;

public class CampaignOfferingsSitemapEntriesProvider : ISitemapEntriesProvider {
    private const string AppealsSection = "appeals";

    private readonly ICdnClient _cdnClient;
    private readonly IClock _clock;
    private readonly IUrlBuilder _urlBuilder;
    private readonly ICampaignOfferingVisibility _visibility;

    public CampaignOfferingsSitemapEntriesProvider(ICdnClient cdnClient,
                                                   IClock clock,
                                                   IUrlBuilder urlBuilder,
                                                   ICampaignOfferingVisibility visibility) {
        _cdnClient = cdnClient;
        _clock = clock;
        _urlBuilder = urlBuilder;
        _visibility = visibility;
    }

    public async Task<IEnumerable<SitemapEntry>> GetEntriesAsync(CancellationToken cancellationToken = default) {
        var entries = new List<SitemapEntry>();

        var today = _clock.GetCurrentInstant().InUtc().Date;

        var publishedCampaigns = await _cdnClient.DownloadSubscriptionContentAsync<PublishedCampaigns>(SubscriptionFiles.Campaigns,
                                                                                                       JsonSerializers.JsonProvider,
                                                                                                       cancellationToken);

        foreach (var publishedCampaign in publishedCampaigns.OrEmpty(x => x.Campaigns)) {
            if (!_visibility.IsVisible(publishedCampaign)) {
                continue;
            }

            AddSitemapEntry(entries, publishedCampaign.Url, today);

            foreach (var publishedOffering in publishedCampaign.Offerings.OrEmpty()) {
                if (_visibility.IsVisible(publishedOffering)) {
                    AddSitemapEntry(entries, publishedOffering.Url, today);
                }
            }
        }

        return entries;
    }

    private void AddSitemapEntry(List<SitemapEntry> entries, Uri publishedUrl, LocalDate today) {
        var url = publishedUrl.RebaseOnSiteRoot(_urlBuilder);

        if (!url.HasValue()) {
            return;
        }

        entries.Add(new SitemapEntry(url, null, AppealsSection, today, null));
    }
}
