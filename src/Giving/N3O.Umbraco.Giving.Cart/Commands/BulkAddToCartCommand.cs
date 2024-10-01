using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Giving.Cart.Commands;

public class BulkAddToCartCommand : Request<BulkAddToCartReq, RevisionId> { }
