using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CompleteCrowdfunderMigrationRes {
    // True when the legacy composition was already gone, so there was nothing to do.
    public bool AlreadyCompleted { get; set; }

    public bool Completed { get; set; }
    public IReadOnlyList<string> CompositionRemovedFrom { get; set; }
    public bool LegacyCompositionDeleted { get; set; }

    // Populated when the gate refused to run: every campaign here must be ready before the teardown will proceed.
    public IReadOnlyList<CrowdfunderMigrationCampaignRes> NotReadyCampaigns { get; set; }
}
