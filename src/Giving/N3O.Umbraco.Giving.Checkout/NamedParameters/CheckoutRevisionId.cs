using N3O.Umbraco.Entities;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Giving.Checkout.NamedParameters {
    public class CheckoutRevisionId : NamedParameter<RevisionId> {
        public override string Name => "checkoutRevisionId";
    }
}