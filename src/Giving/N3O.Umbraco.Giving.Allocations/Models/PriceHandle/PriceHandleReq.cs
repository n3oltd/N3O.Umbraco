using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class PriceHandleReq {
    [Name("Amount")]
    public decimal? Amount { get; set; }
    
    [Name("Description")]
    public string Description { get; set; }
}