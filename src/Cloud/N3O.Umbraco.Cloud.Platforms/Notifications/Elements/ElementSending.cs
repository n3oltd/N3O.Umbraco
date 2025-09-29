using Microsoft.AspNetCore.Mvc.Rendering;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Security;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class ElementSending : INotificationAsyncHandler<SendingContentNotification> {
    private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;
    
    public ElementSending(IBackOfficeSecurityAccessor backOfficeSecurityAccessor) {
        _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
    }

    public Task HandleAsync(SendingContentNotification notification, CancellationToken cancellationToken) {
        var isDonationForm = notification.Content.ContentTypeAlias.EqualsInvariant(AliasHelper<DonationFormElementContent>.ContentTypeAlias());
        var isDonateButton = notification.Content.ContentTypeAlias.EqualsInvariant(AliasHelper<DonateButtonElementContent>.ContentTypeAlias());

        if (isDonationForm || isDonateButton) {
            foreach (var variant in notification.Content.Variants) {
                SetPropertiesReadOnly(variant);
                SetEmbedCode(variant, notification.Content.ContentTypeAlias, notification.Content.Key.GetValueOrDefault());
                HideCalculatedTab(variant);
            }
        }
        
        return Task.CompletedTask;
    }
    
    private void SetPropertiesReadOnly(ContentVariantDisplay variant) {
        var calculatedTab = variant.Tabs.Single(x => x.Alias.EqualsInvariant("calculated"));
        var isSystemGenerated = IsSystemGenerated(calculatedTab);
                
        if (isSystemGenerated) {
            var generalTab = variant.Tabs.Single(x => x.Alias.EqualsInvariant("general"));
            var campaignProperty = GetProperty(generalTab, AliasHelper<ElementContent>.PropertyAlias(x => x.Campaign));

            campaignProperty.Readonly = true;
        }
    }
    
    // TODO Moved here from published notification handler to suppress notifications while creating default elements
    private void SetEmbedCode(ContentVariantDisplay variant, string contentTypeAlias, Guid contentId) {
        var type = StaticLookups.GetAll<ElementType>().Single(x => x.ContentTypeAlias.EqualsInvariant(contentTypeAlias));
            
        var tag = new TagBuilder(type.TagName);
        
        tag.Attributes.Add("form-id", contentId.ToString());
        
        var embedTab = variant.Tabs.Single(x => x.Alias.EqualsInvariant("embed"));
        var embedProperty = GetProperty(embedTab, AliasHelper<ElementContent>.PropertyAlias(x => x.EmbedCode));
        
        embedProperty.Value = tag.ToHtmlString();
    }

    private void HideCalculatedTab(ContentVariantDisplay variant) {
        var calculatedTab = variant.Tabs.Single(x => x.Alias.EqualsInvariant("calculated"));

        calculatedTab.Properties.Do(x => x.Readonly = true);
        
        if (_backOfficeSecurityAccessor.BackOfficeSecurity?.CurrentUser?.IsAdmin() == false) {
            variant.Tabs = variant.Tabs.Except(calculatedTab);
        }
    }

    private bool IsSystemGenerated(Tab<ContentPropertyDisplay> calculatedTab) {
        var value = GetProperty(calculatedTab, AliasHelper<ElementContent>.PropertyAlias(x => x.IsSystemGenerated))?.Value?.ToString();
        
        int.TryParse(value, out var result);
        
        return result == 1;
    }
    
    private ContentPropertyDisplay GetProperty(Tab<ContentPropertyDisplay> tab, string alias) {
        return tab.Properties?.SingleOrDefault(x => x.Alias.EqualsInvariant(alias));
    }
}