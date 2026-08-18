using System;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CrowdfunderWithoutEnabledCampaignRes {
    public Guid CampaignId { get; set; }
    public string CampaignName { get; set; }
    public Guid CrowdfunderId { get; set; }
    public string CrowdfunderName { get; set; }
}
