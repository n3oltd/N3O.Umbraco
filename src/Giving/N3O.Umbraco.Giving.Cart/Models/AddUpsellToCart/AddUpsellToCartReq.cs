using N3O.Umbraco.Attributes;

namespace Read.Core.Models; 

public class AddUpsellToCartReq {
    [Name("Upsell Item Id")]
    public string UpsellItemId { get; set; }
}