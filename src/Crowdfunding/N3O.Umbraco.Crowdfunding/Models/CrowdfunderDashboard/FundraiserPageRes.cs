using N3O.Umbraco.Cloud.Engage.Lookups;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserPageRes {
    public string Name { get; set; }
    public string Owner { get; set; }
    public string Url { get; set; }
    public CrowdfunderStatus Status { get; set; }
}