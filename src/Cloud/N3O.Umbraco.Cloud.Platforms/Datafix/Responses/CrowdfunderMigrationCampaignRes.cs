using System;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CrowdfunderMigrationCampaignRes {
    public Guid CampaignId { get; set; }
    public string CampaignName { get; set; }
    public int ExpectedCopies { get; set; }
    public bool HasCrowdfunder { get; set; }
    public int PopulatedCopies { get; set; }
    public bool Ready { get; set; }
}
