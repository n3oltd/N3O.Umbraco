using Humanizer;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms;

// TODO Delete this class along with the legacy crowdfunding composition once every site has completed the
// migration; it only exists to tell the backfill where the legacy content lives.
public static class CrowdfunderContentSources {
    // The campaign's page content comes first because that is where editors authored crowdfunder content on every
    // site but one, and it is populated on essentially every campaign, so it would win a tie regardless.
    private static readonly List<string> SourceAliases = [
        PlatformsConstants.Campaigns.Properties.PageContent,
        PlatformsConstants.CrowdfundingCampaign.Properties.Content
    ];

    private static readonly List<string> CarriedOverAliases = [];

    public static IReadOnlyList<string> All => SourceAliases;
    public static IReadOnlyList<string> CarriedOver => CarriedOverAliases;

    // The schema and the backfill both need these, so the formula lives in one place. A carried-over property
    // lands on both tabs and a property alias is unique per document type, so the campaign's alias is prefixed
    // the same way the page and template content properties are.
    public static string PageAlias(string carriedOverAlias) {
        return $"page{carriedOverAlias.Pascalize()}";
    }

    public static string PageTemplateAlias(string carriedOverAlias) {
        return $"pageTemplate{carriedOverAlias.Pascalize()}";
    }

    // Called from a site's own composer for a campaign property the crowdfunder page is built from but which never
    // lived on the crowdfunding tab, so the crowdfunder stops reaching back to its campaign for it. Recreated on
    // the crowdfunder with the campaign's own alias, name, data type and tab name.
    public static void CarriesOverCampaignProperty(string alias) {
        if (!CarriedOverAliases.Contains(alias)) {
            CarriedOverAliases.Add(alias);
        }
    }

    // Called from the composer of a site whose editors used the crowdfunding tab as intended, so the campaign's
    // own page content does not win over it.
    public static void PrefersCrowdfundingTabContent() {
        var alias = PlatformsConstants.CrowdfundingCampaign.Properties.Content;

        SourceAliases.Remove(alias);
        SourceAliases.Insert(0, alias);
    }
}
