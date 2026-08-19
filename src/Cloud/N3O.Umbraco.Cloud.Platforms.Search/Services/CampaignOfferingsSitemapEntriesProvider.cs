using Flurl;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
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
    private readonly IUrlBuilder _urlBuilder;
    private readonly IClock _clock;

    public CampaignOfferingsSitemapEntriesProvider(ICdnClient cdnClient,
                                                   IUrlBuilder urlBuilder,
                                                   IClock clock) {
        _cdnClient = cdnClient;
        _urlBuilder = urlBuilder;
        _clock = clock;
    }

    public async Task<IEnumerable<SitemapEntry>> GetEntriesAsync(CancellationToken cancellationToken = default) {
        var entries = new List<SitemapEntry>();

        var today = _clock.GetCurrentInstant().InUtc().Date;

        var publishedCampaigns = await _cdnClient.DownloadSubscriptionContentAsync<PublishedCampaigns>(SubscriptionFiles.Campaigns,
                                                                                                       JsonSerializers.JsonProvider,
                                                                                                       cancellationToken);

        foreach (var publishedCampaign in publishedCampaigns.OrEmpty(x => x.Campaigns)) {
            AddSitemapEntry(entries, publishedCampaign.Url, today);

            foreach (var publishedOffering in publishedCampaign.Offerings.OrEmpty()) {
                AddSitemapEntry(entries, publishedOffering.Url, today);
            }
        }

        return entries;
    }

    private void AddSitemapEntry(List<SitemapEntry> entries, Uri publishedUrl, LocalDate today) {
        var url = RebaseOnSiteRoot(publishedUrl);

        if (!url.HasValue()) {
            return;
        }

        entries.Add(new SitemapEntry(url, null, AppealsSection, today, null));
    }

    private string RebaseOnSiteRoot(Uri url) {
        if (!url.HasValue()) {
            return null;
        }

        var rootUrl = _urlBuilder.Root();
        var rebasedUrl = new Url(url.IsAbsoluteUri ? url.AbsolutePath : url.OriginalString);

        rebasedUrl.Scheme = rootUrl.Scheme;
        rebasedUrl.Host = rootUrl.Host;
        rebasedUrl.Port = rootUrl.Port;

        return rebasedUrl;
    }
}
