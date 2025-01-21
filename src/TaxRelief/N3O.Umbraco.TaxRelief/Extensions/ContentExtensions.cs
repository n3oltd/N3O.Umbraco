using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.TaxRelief.Extensions;

public static class ContentExtensions {
    public static bool IsTaxReliefOrChildSettings(this IContent content, IContentService contentService) {
        var parent = contentService.GetParent(content);
        
        return content.ContentType.Alias.EqualsInvariant(TaxReliefConstants.TaxReliefSettings.Alias) ||
               parent?.ContentType.Alias.EqualsInvariant(TaxReliefConstants.TaxReliefSettings.Alias) == true;
    }
}