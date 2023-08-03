using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Cart.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Giving.Cart.Commands; 

public class RemoveUpsellFromCartCommand : Request<None, RevisionId> {
    public RemoveUpsellFromCartCommand(UpsellOfferId upsellOfferId) {
        UpsellOfferId = upsellOfferId;
    }

    public UpsellOfferId UpsellOfferId { get; }
}
    