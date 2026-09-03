using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json.Linq;
using Slugify;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class CrowdfunderSending : INotificationAsyncHandler<SendingContentNotification> {
    private readonly Lazy<IContentCache> _contentCache;
    private readonly Lazy<ISlugHelper> _slugHelper;
    private readonly Lazy<ILookups> _lookups;

    public CrowdfunderSending(Lazy<IContentCache> contentCache, Lazy<ISlugHelper> slugHelper, Lazy<ILookups> lookups) {
        _contentCache = contentCache;
        _slugHelper = slugHelper;
        _lookups = lookups;
    }

    public Task HandleAsync(SendingContentNotification notification, CancellationToken cancellationToken) {
        var alias = AliasHelper<CrowdfunderContent>.ContentTypeAlias();

        if (notification.Content.ContentTypeAlias.EqualsInvariant(alias)) {
            foreach (var variant in notification.Content.Variants) {
                FixCampaignPicker(variant);
                SetUrl(notification, variant);
            }
        }

        return Task.CompletedTask;
    }

    private void FixCampaignPicker(ContentVariantDisplay variant) {
        var alias = AliasHelper<CrowdfunderContent>.PropertyAlias(y => y.Campaign);
        var campaignProperty = variant.Tabs.SelectMany(x => x.Properties).Single(x => x.Alias == alias);

        if (UdiParser.TryParse(campaignProperty.Value?.ToString(), out var udi)) {
            var contentKey = udi.ToId();
            var campaign = _lookups.Value.FindById<Campaign>(contentKey.ToString());

            campaignProperty.Value = new JArray(campaign.Id);
        }
    }

    private void SetUrl(SendingContentNotification notification, ContentVariantDisplay variant) {
        if (variant.State == ContentSavedState.Published) {
            var path = _contentCache.Value.GetCrowdfundingCampaignPath(_slugHelper.Value, variant.Name);

            if (path.HasValue()) {
                notification.SetPlatformsUrls(_contentCache.Value, path);
            }
        }
    }
}