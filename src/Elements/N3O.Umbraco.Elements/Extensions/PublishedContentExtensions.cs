using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Elements.Extensions;

public static class PublishedContentExtensions {
    public static bool IsDonationCategory(this IPublishedContent content) {
        if (content.ContentType.CompositionAliases.Contains(ElementsConstants.DonationCategory.Alias) ||
            content.ContentType.Alias.EqualsInvariant(ElementsConstants.DonationCategory.Alias)) {
            return true;
        }
        
        return false;
    }
    
    public static bool IsDonationOption(this IPublishedContent content) {
        return content.ContentType.CompositionAliases.Contains(ElementsConstants.DonationOption.Alias);
    }
}