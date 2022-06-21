using N3O.Umbraco.Entities;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Giving.Cart.NamedParameters;

public class CartId : NamedParameter<EntityId> {
    public override string Name => "cartId";
}
