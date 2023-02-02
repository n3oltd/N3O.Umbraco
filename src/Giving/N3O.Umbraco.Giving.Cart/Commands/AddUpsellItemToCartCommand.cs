using N3O.Umbraco.Entities;
using N3O.Umbraco.Mediator;
using Read.Core.Models;

namespace Read.Core.Commands; 

public class AddUpsellItemToCartCommand : Request<AddUpsellToCartReq, RevisionId> {
}