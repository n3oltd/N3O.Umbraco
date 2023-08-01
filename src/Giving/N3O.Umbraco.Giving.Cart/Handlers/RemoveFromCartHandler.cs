using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Extensions;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Mediator;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Handlers;

public class RemoveFromCartHandler :
    IRequestHandler<RemoveFromCartCommand, RemoveFromCartReq, RevisionId>,
    IRequestHandler<RemoveUpsellFromCartCommand, None, RevisionId> {
    private readonly ICartAccessor _cartAccessor;
    private readonly IRepository<Entities.Cart> _repository;

    public RemoveFromCartHandler(ICartAccessor cartAccessor, IRepository<Entities.Cart> repository) {
        _cartAccessor = cartAccessor;
        _repository = repository;
    }

    public async Task<RevisionId> Handle(RemoveFromCartCommand req, CancellationToken cancellationToken) {
        var cart = await _cartAccessor.GetAsync(cancellationToken);

        cart.Remove(req.Model.GivingType, req.Model.Index.GetValueOrThrow());

        await _repository.UpdateAsync(cart);

        return cart.RevisionId;
    }

    public async Task<RevisionId> Handle(RemoveUpsellFromCartCommand req, CancellationToken cancellationToken) {
        var cart = await _cartAccessor.GetAsync(cancellationToken);

        if (!cart.ContainsUpsell(req.UpsellId.Value)) {
            throw new Exception($"The upsell with id {req.UpsellId.Value} does not exist in the cart");
        }
        
        cart.RemoveUpsell(cart.Donation, req.UpsellId.Value);
            
        await _repository.UpdateAsync(cart);

        return cart.RevisionId;
    }
}