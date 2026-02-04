using Microsoft.AspNetCore.Mvc.Rendering;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Slugify;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class OfferingSending : INotificationAsyncHandler<SendingContentNotification> {
    private readonly Lazy<IContentCache> _contentCache;
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly Lazy<ISlugHelper> _slugHelper;
    
    public OfferingSending(Lazy<IContentCache> contentCache,
                           Lazy<IContentLocator> contentLocator,
                           Lazy<ISlugHelper> slugHelper) {
        _contentCache = contentCache;
        _contentLocator = contentLocator;
        _slugHelper = slugHelper;
    }

    public Task HandleAsync(SendingContentNotification notification, CancellationToken cancellationToken) {
        var isOffering = notification.Content
                                     .ContentTypeAlias
                                     .IsAnyOf(AliasHelper<FundOfferingContent>.ContentTypeAlias(),
                                              AliasHelper<SponsorshipOfferingContent>.ContentTypeAlias(),
                                              AliasHelper<FeedbackOfferingContent>.ContentTypeAlias());

        if (isOffering) {
            foreach (var variant in notification.Content.Variants) {
                SetEmbedCode(variant, notification.Content.Key.GetValueOrDefault());
                SetUrl( notification, variant);
            }
        }
        
        return Task.CompletedTask;
    }
    
    private void SetUrl(SendingContentNotification notification, ContentVariantDisplay variant) {
        if (variant.State == ContentSavedState.Published) {
            var campaignName = _contentLocator.Value.ById(notification.Content.ParentId.GetValueOrThrow()).Name;

            var offeringPath = _contentCache.Value.GetOfferingPath(_slugHelper.Value, campaignName, variant.Name);

            if (offeringPath.HasValue()) {
                notification.Content.Urls = [new UrlInfo(offeringPath, true, null)];
            }
        }
    }
    
    private void SetEmbedCode(ContentVariantDisplay variant, Guid contentId) {
        var donationFormTag = new TagBuilder(ElementTypes.DonationForm.TagName);
        var donationButtonTag = new TagBuilder(ElementTypes.DonationButton.TagName);

        donationFormTag.Attributes.Add("element-id", $"{contentId.ToString()}");
        donationButtonTag.Attributes.Add("element-id", $"{contentId.ToString()}");

        donationFormTag.Attributes.Add("element-kind", ElementKind.DonationFormOffering.ToEnumString());
        donationButtonTag.Attributes.Add("element-kind", ElementKind.DonationButtonOffering.ToEnumString());
        
        var embedTab = variant.Tabs
                              .SingleOrDefault(x => x.Properties.OrEmpty().Any(y => y.Alias.IsAnyOf(AliasHelper<OfferingContent>.PropertyAlias(z => z.DonationFormEmbedCode),
                                                                                                    AliasHelper<OfferingContent>.PropertyAlias(z => z.DonationButtonEmbedCode))));
        
        var donationFormTagEmbedProperty = GetProperty(embedTab, AliasHelper<OfferingContent>.PropertyAlias(x => x.DonationFormEmbedCode));
        var donationButtonEmbedProperty = GetProperty(embedTab, AliasHelper<OfferingContent>.PropertyAlias(x => x.DonationButtonEmbedCode));
        
        donationFormTagEmbedProperty.IfNotNull(x => x.Value = donationFormTag.ToHtmlString());
        donationButtonEmbedProperty.IfNotNull(x => x.Value = donationButtonTag.ToHtmlString());
    }
    
    private ContentPropertyDisplay GetProperty(Tab<ContentPropertyDisplay> tab, string alias) {
        return tab?.Properties?.SingleOrDefault(x => x.Alias.EqualsInvariant(alias));
    }
}
