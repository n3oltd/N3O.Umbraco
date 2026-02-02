using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Search;
using N3O.Umbraco.Search.Models;
using NodaTime;
using Slugify;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms.Search;

public class CampaignOfferingsSitemapEntriesProvider : ISitemapEntriesProvider {
    private readonly ICdnClient _cdnClient;
    private readonly IContentCache _contentCache;
    private readonly ISlugHelper _slugHelper;
    private readonly IClock _clock;

    public CampaignOfferingsSitemapEntriesProvider(ICdnClient cdnClient,
                                                   IContentCache contentCache,
                                                   ISlugHelper slugHelper,
                                                   IClock clock) {
        _cdnClient = cdnClient;
        _contentCache = contentCache;
        _slugHelper = slugHelper;
        _clock = clock;
    }
    
    public async Task<IEnumerable<SitemapEntry>> GetEntriesAsync(CancellationToken cancellationToken = default) {
        var entries = new List<SitemapEntry>();

        var today = _clock.GetCurrentInstant().InUtc().Date;
        
        var publishedCampaigns = await _cdnClient.DownloadSubscriptionContentAsync<PublishedCampaigns>(SubscriptionFiles.Campaigns,
                                                                                                       JsonSerializers.JsonProvider,
                                                                                                       cancellationToken);

        foreach (var publishedCampaign in publishedCampaigns.Campaigns) {
            entries.Add(GetSitemapEntryForCampaign(publishedCampaign, today));

            foreach (var publishedOffering in publishedCampaign.Offerings) {
                entries.Add(GetSitemapEntryForOffering(publishedOffering, publishedCampaign, today));
            }
        }

        return entries;
    }

    private SitemapEntry GetSitemapEntryForCampaign(PublishedCampaign publishedCampaign, LocalDate today) {
        var url = _contentCache.GetCampaignUrl(_slugHelper, publishedCampaign.Name);

        return new SitemapEntry(url, "daily", 0.5f, today);
    }
    
    private SitemapEntry GetSitemapEntryForOffering(PublishedOffering publishedOffering,
                                                    PublishedCampaign publishedCampaign,
                                                    LocalDate today) {
        var url = _contentCache.GetOfferingUrl(_slugHelper, publishedCampaign.Name, publishedOffering.Name);

        return new SitemapEntry(url, "daily", 0.5f, today);
    }
}