namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CrowdfunderMigrationSummaryRes {
    public int EnabledCampaignsCount { get; set; }
    public int FallbackSourceCount { get; set; }
    public int NotReadyCount { get; set; }
    public int ReadyCount { get; set; }
}
