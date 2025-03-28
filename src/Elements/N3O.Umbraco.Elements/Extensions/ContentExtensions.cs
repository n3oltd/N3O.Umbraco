using N3O.Umbraco.Accounts.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Elements.Content;
using N3O.Umbraco.TaxRelief.Extensions;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Elements.Extensions;

public static class ContentExtensions {
    public static bool IsDonationCategory(this IContent content, IContentTypeService contentTypeService) {
        return HasComposition(content, contentTypeService, ElementsConstants.DonationCategory.CompositionAlias);
    }
    
    public static bool IsDonationOption(this IContent content, IContentTypeService contentTypeService) {
        return HasComposition(content, contentTypeService, ElementsConstants.DonationOption.CompositionAlias);
    }
    
    public static bool IsCheckoutProfileDependency(this IContent content, IContentService contentService) {
        return content.IsDataEntrySettingsOrChild(contentService) ||
               content.ElementsCheckoutCompleteSettings() ||
               content.IsConsentSettingsOrDescendant(contentService) ||
               content.IsTaxReliefOrChildSettings(contentService) ||
               content.IsPreferencesDataEntrySettings();
    }
    
    public static bool IsElementsSettings(this IContent content) {
        return content.ContentType.Alias.EqualsInvariant(ElementsConstants.ElementsSettings.Alias);
    }
    
    private static bool ElementsCheckoutCompleteSettings(this IContent content) {
        return content.ContentType.Alias.EqualsInvariant(ElementsConstants.ElementsCheckoutCompleteSettings.Alias);
    }
    
    public static bool IsPreferencesDataEntrySettings(this IContent content) {
        return content.ContentType.Alias.EqualsInvariant(AliasHelper<PreferencesDataEntrySettingsContent>.ContentTypeAlias());
    }
    
    private static bool HasComposition(this IContent content, IContentTypeService contentTypeService, string alias) {
        var contentType = contentTypeService.Get(content.ContentType.Id);
        
        return contentType.ContentTypeCompositionExists(alias);
    }
}