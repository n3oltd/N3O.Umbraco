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

public class CrowdfundingCampaignSending : INotificationAsyncHandler<SendingContentNotification> {
    private readonly Lazy<IContentCache> _contentCache;
    private readonly Lazy<ISlugHelper> _slugHelper;

    public CrowdfundingCampaignSending(Lazy<IContentCache> contentCache, Lazy<ISlugHelper> slugHelper) {
        _contentCache = contentCache;
        _slugHelper = slugHelper;
    }

    public Task HandleAsync(SendingContentNotification notification, CancellationToken cancellationToken) {
        var alias = AliasHelper<CrowdfundingCampaignContent>.ContentTypeAlias();

        if (notification.Content.ContentTypeAlias.EqualsInvariant(alias)) {
            foreach (var variant in notification.Content.Variants) {
                SetUrl(notification, variant);
                FixCampaignPicker(variant);
            }
        }

        return Task.CompletedTask;
    }

    // TODO Delete once every crowdfunding campaign node has been saved with a data list value
    private void FixCampaignPicker(ContentVariantDisplay variant) {
        var alias = AliasHelper<CrowdfundingCampaignContent>.PropertyAlias(y => y.Campaign);
        var campaignProperty = variant.Tabs
                                      .SelectMany(x => x.Properties.OrEmpty())
                                      .SingleOrDefault(x => x.Alias == alias);

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
