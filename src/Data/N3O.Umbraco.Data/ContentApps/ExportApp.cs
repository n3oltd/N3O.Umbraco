using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.Membership;
using UmbracoSecurity = Umbraco.Cms.Core.Constants.Security;

namespace N3O.Umbraco.Data.ContentApps {
    public class ExportApp : IContentAppFactory {
        private readonly IEnumerable<IExportContentFilter> _contentFilters;

        public ExportApp(IEnumerable<IExportContentFilter> contentFilters) {
            _contentFilters = contentFilters.OrEmpty().ToList();
        }

        public ContentApp GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups) {
            if (userGroups.All(x => x.Alias.ToLowerInvariant() != UmbracoSecurity.AdminGroupAlias && 
                                    x.Name != DataConstants.SecurityGroups.ExportUser)) {
                return null;
            }

            var content = source as IContent;

            if (content == null || content.Id == default) {
                return null;
            }
            
            var filter = _contentFilters.SingleOrDefault(x => x.IsFilter(content));

            if (filter?.AllowExports(content) == true) {
                return new ContentApp {
                    Alias = "export",
                    Name = "Export",
                    Icon = "icon-arrow-down",
                    View = "/App_Plugins/N3O.Umbraco.Data.Export/N3O.Umbraco.Data.Export.html",
                    Weight = -99
                }; 
            } else {
                return null;
            }
        }
    }
}