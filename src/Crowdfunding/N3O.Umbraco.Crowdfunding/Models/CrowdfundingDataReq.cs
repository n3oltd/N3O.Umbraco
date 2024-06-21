using N3O.Umbraco.Attributes;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingDataReq {
    [Name("Campaign ID")]
    public Guid? CampaignId { get; set; }
    
    [Name("Team ID")]
    public Guid? TeamId { get; set; }

    [Name("Page ID")]
    public Guid? PageId { get; set; }

    [Name("Comment")]
    public string Comment { get; set; }

    [Name("Anonymous")]
    public bool Anonymous { get; set;  }
}