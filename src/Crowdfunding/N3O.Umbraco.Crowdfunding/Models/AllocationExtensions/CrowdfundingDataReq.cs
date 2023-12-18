using N3O.Umbraco.Attributes;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public interface ICrowdfundingData {
    
}

public class CrowdfundingDataReq : ICrowdfundingData {
    [Name("Campaign ID")]
    public Guid? CampaignId { get; set; }
    
    [Name("Team ID")]
    public Guid? TeamId { get; set; }
    
    [Name("Page ID")]
    public Guid? PageId { get; set; }
}