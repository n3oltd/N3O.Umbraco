using N3O.Umbraco.Crowdfunding.Models.AddToCart;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Commands;

public class AddToCartCommand : Request<AddToCartReq, RevisionId> { }