using N3O.Umbraco.Accounts.Extensions;
using N3O.Umbraco.TaxRelief.Extensions;
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
    
    public static bool IsCheckoutProfileReliefElement(this IContent content,
                                                      IContentService contentService) {
        return content.IsDataEntrySettingsOrChild(contentService) ||
               content.IsConsentSettingsOrDescendant(contentService) ||
               content.IsTaxReliefOrChildSettings(contentService);
    }
    
    private static bool HasComposition(this IContent content, IContentTypeService contentTypeService, string alias) {
        var contentType = contentTypeService.Get(content.ContentType.Id);
        
        return contentType.ContentTypeCompositionExists(alias);
    }
}