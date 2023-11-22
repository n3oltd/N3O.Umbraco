using System;

namespace N3O.Umbraco.CrowdFunding.Models; 
/*
 * Add extension methods similar to BlockViewModelExtensions which allows us to get or set
 * a strongly typed version of this into an allocation
 */
public class CrowdfundingAllocationData {
    public Guid PageId { get; }
    public string PageUrl { get; }
    public string Comment { get; }
    public string Name { get; }
    public bool HideName { get; }
}