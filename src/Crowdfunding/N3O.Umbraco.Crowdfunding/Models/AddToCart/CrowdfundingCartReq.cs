using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Engage.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingCartReq {
    [Name("Items")]
    public IEnumerable<CrowdfundingCartItemReq> Items { get; set; }
    
    [Name("Type")]
    public CrowdfunderType Type { get; set; }
    
    [Name("Crowdfunding")]
    public CrowdfunderDataReq Crowdfunding { get; set; }
}