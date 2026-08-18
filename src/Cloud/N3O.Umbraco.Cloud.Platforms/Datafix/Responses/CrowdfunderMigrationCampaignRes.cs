using System;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CrowdfunderMigrationCampaignRes {
    public Guid CampaignId { get; set; }
    public string CampaignName { get; set; }

    // How many of the site's declared copies had a source to copy from, and how many of those reached the
    // crowdfunder. They match once the migration has done everything it was asked to.
    public int ExpectedCopies { get; set; }
    public int PopulatedCopies { get; set; }

    public bool HasCrowdfunder { get; set; }
    public bool Ready { get; set; }
}
