using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CrowdfunderMigrationStatusRes {
    public IReadOnlyList<CrowdfunderMigrationCampaignRes> Campaigns { get; set; }
    public IReadOnlyList<CrowdfunderWithoutEnabledCampaignRes> CrowdfundersWithoutEnabledCampaign { get; set; }
    public CrowdfunderMigrationSummaryRes Summary { get; set; }
}
