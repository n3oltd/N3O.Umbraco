using System;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CrowdfunderMigrationCampaignRes {
    public Guid CampaignId { get; set; }
    public string CampaignName { get; set; }
    public bool HasCrowdfunder { get; set; }
    public bool PageTemplatePopulated { get; set; }
    public bool Ready { get; set; }
    public string TemplateSourceAlias { get; set; }

    // True when the template came from anywhere other than the crowdfunding tab.
    public bool UsedFallbackSource { get; set; }
}
