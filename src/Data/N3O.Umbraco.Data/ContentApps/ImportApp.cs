using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.Membership;
using UmbracoSecurity = Umbraco.Cms.Core.Constants.Security;


namespace N3O.Umbraco.Data.ContentApps;

public class ImportApp : IContentAppFactory {
    private readonly IEnumerable<IImportContentFilter> _contentFilters;

    public ImportApp(IEnumerable<IImportContentFilter> contentFilters) {
        _contentFilters = contentFilters.OrEmpty().ToList();
    }

    public ContentApp GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups) {
        if (userGroups.All(x => x.Alias.ToLowerInvariant() != UmbracoSecurity.AdminGroupAlias && 
                                x.Name != DataConstants.SecurityGroups.ImportUsers)) {
            return null;
        }

        var content = source as IContent;

        if (content == null || content.Id == default) {
            return null;
        }
        
        var filter = _contentFilters.SingleOrDefault(x => x.IsFilter(content));

        if (filter?.AllowImports(content) == true) {
            return new ContentApp {
                Alias = "import",
                Name = "Import",
                Icon = "icon-arrow-up",
                View = "/App_Plugins/N3O.Umbraco.Data.Import/N3O.Umbraco.Data.Import.html",
                Weight = -98
            };
        } else {
            return null;
        }
    }
}
