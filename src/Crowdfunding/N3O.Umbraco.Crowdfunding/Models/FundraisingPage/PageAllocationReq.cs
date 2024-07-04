using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class PageAllocationReq : AllocationReq {
    [Name("Title")]
    public string Title { get; set; }
    
    [Name("Amount")]
    public decimal Amount { get; set; }
    
    [Name("Price Handles")]
    public IEnumerable<PriceHandleReq> PriceHandles { get; set; }
}