using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search;
using N3O.Umbraco.Search.Models;
using Slugify;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms.Search;

public class CampaignOfferingsSitemapEntriesProvider : ISitemapEntriesProvider {
    private const string AppealsSection = "appeals";

    private readonly ICdnClient _cdnClient;
    private readonly IContentCache _contentCache;
    private readonly ISlugHelper _slugHelper;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CampaignOfferingsSitemapEntriesProvider(ICdnClient cdnClient,
                                                   IContentCache contentCache,
                                                   ISlugHelper slugHelper,
                                                   IWebHostEnvironment webHostEnvironment) {
        _cdnClient = cdnClient;
        _contentCache = contentCache;
        _slugHelper = slugHelper;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IEnumerable<SitemapEntry>> GetEntriesAsync(CancellationToken cancellationToken = default) {
        var entries = new List<SitemapEntry>();

        var publishedCampaigns = await _cdnClient.DownloadSubscriptionContentAsync<PublishedCampaigns>(SubscriptionFiles.Campaigns,
                                                                                                       JsonSerializers.JsonProvider,
                                                                                                       cancellationToken);

        foreach (var publishedCampaign in publishedCampaigns.OrEmpty(x => x.Campaigns)) {
            entries.Add(GetSitemapEntryForCampaign(publishedCampaign));

            foreach (var publishedOffering in publishedCampaign.Offerings) {
                entries.Add(GetSitemapEntryForOffering(publishedOffering, publishedCampaign));
            }
        }

        return entries;
    }

    private SitemapEntry GetSitemapEntryForCampaign(PublishedCampaign publishedCampaign) {
        var url = _contentCache.GetCampaignUrl(_slugHelper, _webHostEnvironment, publishedCampaign.Name);

        return new SitemapEntry(url, null, AppealsSection, null, null);
    }

    private SitemapEntry GetSitemapEntryForOffering(PublishedOffering publishedOffering,
                                                    PublishedCampaign publishedCampaign) {
        var url = _contentCache.GetOfferingUrl(_slugHelper,
                                               _webHostEnvironment,
                                               publishedCampaign.Name,
                                               publishedOffering.Name);

        return new SitemapEntry(url, null, AppealsSection, null, null);
    }
}