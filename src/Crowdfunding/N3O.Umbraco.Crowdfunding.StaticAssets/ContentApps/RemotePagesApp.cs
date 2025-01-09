using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.Membership;

namespace N3O.Umbraco.Crowdfunding;

public class RemotePagesApp : IContentAppFactory {
    public ContentApp GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups) {
        var content = source as IContent;

        if (content == null || content.Id == default) {
            return null;
        }

        if (!content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Fundraisers.Alias)) {
            return null;
        }

        return new ContentApp {
            Alias = "crowdfundingRemotePages",
            Name = "RemotePages",
            Icon = "icon-pie-chart",
            View = "/App_Plugins/N3O.Umbraco.Crowdfunding.RemotePages/N3O.Umbraco.Crowdfunding.RemotePages.html",
            Weight = -999
        };
    }
}