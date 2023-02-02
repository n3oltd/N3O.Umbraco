using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Cart.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Giving.Cart.Commands;

public class AddUpsellToCartCommand : Request<None, RevisionId> {
    public AddUpsellToCartCommand(UpsellId upsellId) {
        UpsellId = upsellId;
    }

    public UpsellId UpsellId { get; }
}