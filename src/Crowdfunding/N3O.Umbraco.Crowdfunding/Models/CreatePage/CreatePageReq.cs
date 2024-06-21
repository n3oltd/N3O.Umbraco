using N3O.Umbraco.Attributes;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CreatePageReq {
    [Name("Name")]
    public string Name { get; set; }
    
    [Name("Campaign ID")]
    public Guid? CampaignId { get; set; }
}