namespace N3O.Umbraco.Cloud.Platforms.Models;

public class ContentUrlsRes {
    public bool Permitted { get; set; }
    public string StagingUrl { get; set; }
    public string ProductionUrl { get; set; }
}
