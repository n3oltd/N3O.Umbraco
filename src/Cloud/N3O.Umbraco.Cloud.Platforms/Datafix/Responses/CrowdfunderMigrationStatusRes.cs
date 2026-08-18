using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CrowdfunderMigrationStatusRes {
    public IReadOnlyList<CrowdfunderMigrationCampaignRes> Campaigns { get; set; }

    // Crowdfunders whose picked campaign is not crowdfunding-enabled. Not an error, but a state worth seeing
    // before tearing the legacy composition down.
    public IReadOnlyList<CrowdfunderWithoutEnabledCampaignRes> CrowdfundersWithoutEnabledCampaign { get; set; }

    public CrowdfunderMigrationSummaryRes Summary { get; set; }
}
