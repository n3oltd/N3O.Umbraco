using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Cart.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Giving.Cart.Commands;

public class ClearCartCommand : Request<ClearCartReq, None> {
    public ClearCartCommand(CartId cartId) {
        CartId = cartId;
    }
    
    public CartId CartId { get; }
}
