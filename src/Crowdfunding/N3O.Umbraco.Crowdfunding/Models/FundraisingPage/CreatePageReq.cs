using N3O.Umbraco.Attributes;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CreatePageReq {
    [Name("Name")]
    public string Name { get; set; }
    
    [Name("Slug")]
    public string Slug { get; set; }
    
    [Name("Campaign ID")]
    public Guid? CampaignId { get; set; }
    
    [Name("Allocation")]
    public IEnumerable<PageAllocationReq> Allocation { get; set; }
}