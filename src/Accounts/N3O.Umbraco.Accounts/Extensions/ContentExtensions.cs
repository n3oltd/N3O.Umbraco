using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Accounts.Extensions;

public static class ContentExtensions {
    public static bool IsConsentSettingsOrDescendant(this IContent content, IContentService contentService) {
        var ancestors = contentService.GetAncestors(content);
        
        return content.ContentType.Alias.EqualsInvariant(AccountsConstants.ConsentSettings.Alias) ||
               ancestors.Any(x => x.ContentType.Alias.EqualsInvariant(AccountsConstants.ConsentSettings.Alias));
    }
    
    public static bool IsDataEntrySettingsOrChild(this IContent content, IContentService contentService) {
        var parent = contentService.GetParent(content);
        
        return content.ContentType.Alias.EqualsInvariant(AccountsConstants.DataEntrySettings.Alias) ||
               parent?.ContentType.Alias.EqualsInvariant(AccountsConstants.DataEntrySettings.Alias) == true;
    }
}