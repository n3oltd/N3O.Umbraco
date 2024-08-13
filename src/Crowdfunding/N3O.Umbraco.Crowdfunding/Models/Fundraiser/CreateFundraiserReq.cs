using N3O.Umbraco.Attributes;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CreateFundraiserReq {
    [Name("Title")]
    public string Title { get; set; }
    
    [Name("Slug")]
    public string Slug { get; set; }
    
    [Name("Campaign ID")]
    public Guid? CampaignId { get; set; }
    
    [Name("Allocations")]
    public IEnumerable<FundraiserAllocationReq> Allocations { get; set; }
}