using Microsoft.AspNetCore.Mvc.Rendering;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class OfferingSending : INotificationAsyncHandler<SendingContentNotification> {
    public Task HandleAsync(SendingContentNotification notification, CancellationToken cancellationToken) {
        var isOffering = notification.Content
                                     .ContentTypeAlias
                                     .IsAnyOf(AliasHelper<FundOfferingContent>.ContentTypeAlias(),
                                              AliasHelper<SponsorshipOfferingContent>.ContentTypeAlias(),
                                              AliasHelper<FeedbackOfferingContent>.ContentTypeAlias());

        if (isOffering) {
            foreach (var variant in notification.Content.Variants) {
                SetEmbedCode(variant, notification.Content.Key.GetValueOrDefault());
            }
        }
        
        return Task.CompletedTask;
    }
    
    private void SetEmbedCode(ContentVariantDisplay variant, Guid contentId) {
        var donationFormTag = new TagBuilder(ElementTypes.DonationForm.TagName);
        var donationButtonTag = new TagBuilder(ElementTypes.DonationButton.TagName);

        donationFormTag.Attributes.Add("element-id", $"{ElementKind.DonationFormOffering.ToEnumString()}_{contentId.ToString()}");
        donationButtonTag.Attributes.Add("element-id", $"{ElementKind.DonationButtonOffering.ToEnumString()}_{contentId.ToString()}");

        donationFormTag.Attributes.Add("element-type", ElementKind.DonationFormOffering.ToEnumString());
        donationButtonTag.Attributes.Add("element-type", ElementKind.DonationButtonOffering.ToEnumString());
        
        var embedTab = variant.Tabs.SingleOrDefault(x => x.Alias.EqualsInvariant("embed"));
        
        var donationFormTagEmbedProperty = GetProperty(embedTab, AliasHelper<OfferingContent>.PropertyAlias(x => x.DonationFormEmbedCode));
        var donationButtonEmbedProperty = GetProperty(embedTab, AliasHelper<OfferingContent>.PropertyAlias(x => x.DonationButtonEmbedCode));
        
        donationFormTagEmbedProperty.Value = donationFormTag.ToHtmlString();
        donationButtonEmbedProperty.Value = donationButtonTag.ToHtmlString();
    }
    
    private ContentPropertyDisplay GetProperty(Tab<ContentPropertyDisplay> tab, string alias) {
        return tab?.Properties?.SingleOrDefault(x => x.Alias.EqualsInvariant(alias));
    }
}