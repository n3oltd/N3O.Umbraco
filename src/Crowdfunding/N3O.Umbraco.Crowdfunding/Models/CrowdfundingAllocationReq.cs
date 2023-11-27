using System;

namespace N3O.Umbraco.CrowdFunding.Models; 

public class CrowdfundingAllocationReq {
    public Guid PageId { get; }
    public string PageUrl { get; }
    public string Comment { get; }
    public string Name { get; }
    public bool HideName { get; }
}