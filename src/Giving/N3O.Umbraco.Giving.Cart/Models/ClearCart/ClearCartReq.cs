using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Cart.Models;

public class ClearCartReq {
    [Name("Giving Type")]
    public GivingType GivingType { get; set; }
}
