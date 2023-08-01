using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Giving.Cart.Models; 

public class AddUpsellToCartReq {
    [Name("Amount")]
    public decimal? Amount { get; set; }
}