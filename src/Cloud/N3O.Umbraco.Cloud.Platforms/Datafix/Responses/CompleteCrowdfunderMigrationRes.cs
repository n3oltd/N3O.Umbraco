using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CompleteCrowdfunderMigrationRes {
    public bool AlreadyCompleted { get; set; }
    public bool Completed { get; set; }
    public IReadOnlyList<string> CompositionRemovedFrom { get; set; }
    public bool LegacyCompositionDeleted { get; set; }
    public IReadOnlyList<CrowdfunderMigrationCampaignRes> NotReadyCampaigns { get; set; }
}
