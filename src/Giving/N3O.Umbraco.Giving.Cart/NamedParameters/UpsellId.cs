using N3O.Umbraco.Parameters;
using System;

namespace N3O.Umbraco.Giving.Cart.NamedParameters;

public class UpsellId : NamedParameter<Guid> {
    public override string Name => "upsellId";
}
