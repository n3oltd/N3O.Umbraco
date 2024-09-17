using N3O.Umbraco.Attributes;
using N3O.Umbraco.Financial;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CreateFundraiserReq {
    [Name("Name")]
    public string Name { get; set; }
    
    [Name("Slug")]
    public string Slug { get; set; }
    
    [Name("Campaign ID")]
    public Guid? CampaignId { get; set; }
    
    [Name("Currency")]
    public Currency Currency { get; set; }
    
    [Name("Goals")]
    public FundraiserGoalsReq Goals { get; set; }
}