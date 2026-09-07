using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
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

    public CrowdfunderSending(Lazy<IContentCache> contentCache, Lazy<ISlugHelper> slugHelper) {
        _contentCache = contentCache;
        _slugHelper = slugHelper;
    }

    public Task HandleAsync(SendingContentNotification notification, CancellationToken cancellationToken) {
        var alias = AliasHelper<CrowdfunderContent>.ContentTypeAlias();

        if (notification.Content.ContentTypeAlias.EqualsInvariant(alias)) {
            foreach (var variant in notification.Content.Variants) {
                SetUrl(notification, variant);
                FixCampaignPicker(variant);
            }
        }

        return Task.CompletedTask;
    }

    // A node saved while the campaign picker was a content picker holds a document udi, which the data list
    // cannot display. The campaign id is the node key, so the stored value is repaired on the next save.
    private void FixCampaignPicker(ContentVariantDisplay variant) {
        var alias = AliasHelper<CrowdfunderContent>.PropertyAlias(y => y.Campaign);
        var campaignProperty = variant.Tabs.SelectMany(x => x.Properties.OrEmpty()).SingleOrDefault(x => x.Alias == alias);

        if (campaignProperty != null && UdiParser.TryParse(campaignProperty.Value?.ToString(), out GuidUdi udi)) {
            campaignProperty.Value = new JArray(udi.Guid.ToString());
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
