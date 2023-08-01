using N3O.Umbraco.Attributes;
using N3O.Umbraco.Entities;

namespace N3O.Umbraco.Giving.Cart.Models; 

public class AddUpsellToCartReq {
    [Name("Upsell Id")]
    public EntityId UpsellId { get; set; }

    [Name("Amount")]
    public decimal Amount { get; set; }
}