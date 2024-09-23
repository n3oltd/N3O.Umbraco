using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.Membership;

namespace N3O.Umbraco.Crowdfunding;

public class StatisticsApp : IContentAppFactory {
    public ContentApp GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups) {
        var content = source as IContent;

        if (content == null || content.Id == default) {
            return null;
        }

        if (!content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Root.Alias)) {
            return null;
        }

        return new ContentApp {
            Alias = "crowdfundingStatistics",
            Name = "Statistics",
            Icon = "icon-pie-chart",
            View = "/App_Plugins/N3O.Umbraco.Crowdfunding.Statistics/N3O.Umbraco.Crowdfunding.Statistics.html",
            Weight = -999
        };
    }
}