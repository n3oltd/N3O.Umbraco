using CsvHelper.Configuration.Attributes;
using N3O.Umbraco.Crowdfunding.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models.AddToCart;

public class AddToCartReq {
    [Name("Cart Items")]
    public IEnumerable<AddToCartItemReq> Items { get; set; }
    
    [Name("Crowdfunder Type")]
    public CrowdfunderType Type { get; set; }
    
    [Name("Crowdfunding")]
    public CrowdfunderDataReq Crowdfunding { get; set; }
}