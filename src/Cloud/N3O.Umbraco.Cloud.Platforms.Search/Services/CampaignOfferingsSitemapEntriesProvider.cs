using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search;
using N3O.Umbraco.Search.Models;
using NodaTime;
using Slugify;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms.Search;

public class CampaignOfferingsSitemapEntriesProvider : ISitemapEntriesProvider {
    private const string AppealsSection = "appeals";

    private readonly ICdnClient _cdnClient;
    private readonly IClock _clock;
    private readonly IContentCache _contentCache;
    private readonly ISlugHelper _slugHelper;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ICampaignOfferingVisibility _visibility;

    public CampaignOfferingsSitemapEntriesProvider(ICdnClient cdnClient,
                                                   IClock clock,
                                                   IContentCache contentCache,
                                                   ISlugHelper slugHelper,
                                                   IWebHostEnvironment webHostEnvironment,
                                                   ICampaignOfferingVisibility visibility) {
        _cdnClient = cdnClient;
        _clock = clock;
        _contentCache = contentCache;
        _slugHelper = slugHelper;
        _webHostEnvironment = webHostEnvironment;
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

            entries.Add(GetSitemapEntryForCampaign(publishedCampaign, today));

            foreach (var publishedOffering in publishedCampaign.Offerings.OrEmpty()) {
                if (_visibility.IsVisible(publishedOffering)) {
                    entries.Add(GetSitemapEntryForOffering(publishedOffering, publishedCampaign, today));
                }
            }
        }

        return entries;
    }

    private SitemapEntry GetSitemapEntryForCampaign(PublishedCampaign publishedCampaign, LocalDate today) {
        var url = _contentCache.GetCampaignUrl(_slugHelper, _webHostEnvironment, publishedCampaign.Name);

        return new SitemapEntry(url, null, AppealsSection, today, null);
    }

    private SitemapEntry GetSitemapEntryForOffering(PublishedOffering publishedOffering,
                                                    PublishedCampaign publishedCampaign,
                                                    LocalDate today) {
        var url = _contentCache.GetOfferingUrl(_slugHelper,
                                               _webHostEnvironment,
                                               publishedCampaign.Name,
                                               publishedOffering.Name);

        return new SitemapEntry(url, null, AppealsSection, today, null);
    }
}