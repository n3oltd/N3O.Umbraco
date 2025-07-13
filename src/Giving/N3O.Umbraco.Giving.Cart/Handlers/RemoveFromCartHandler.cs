using N3O.Umbraco.Content;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Allocations;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Handlers;

public class RemoveFromCartHandler :
    IRequestHandler<BulkRemoveFromCartCommand, BulkRemoveFromCartReq, RevisionId>,
    IRequestHandler<RemoveFromCartCommand, RemoveFromCartReq, RevisionId>,
    IRequestHandler<RemoveUpsellFromCartCommand, None, RevisionId> {
    private readonly IContentLocator _contentLocator;
    private readonly IForexConverter _forexConverter;
    private readonly IPriceCalculator _priceCalculator;
    private readonly ICartAccessor _cartAccessor;
    private readonly ILookups _lookups;
    private readonly IRepository<Entities.Cart> _repository;

    public RemoveFromCartHandler(IContentLocator contentLocator,
                                 IForexConverter forexConverter,
                                 IPriceCalculator priceCalculator,
                                 ICartAccessor cartAccessor,
                                 ILookups lookups,
                                 IRepository<Entities.Cart> repository) {
        _contentLocator = contentLocator;
        _forexConverter = forexConverter;
        _priceCalculator = priceCalculator;
        _cartAccessor = cartAccessor;
        _lookups = lookups;
        _repository = repository;
    }
    
    public async Task<RevisionId> Handle(BulkRemoveFromCartCommand req, CancellationToken cancellationToken) {
        var cart = await _cartAccessor.GetAsync(cancellationToken);

        await cart.BulkRemoveAsync(_contentLocator,
                                   _forexConverter,
                                   _priceCalculator,
                                   _lookups,
                                   req.Model.GivingType,
                                   req.Model.Indexes);

        await _repository.UpdateAsync(cart);

        return cart.RevisionId;
    }

    public async Task<RevisionId> Handle(RemoveFromCartCommand req, CancellationToken cancellationToken) {
        var cart = await _cartAccessor.GetAsync(cancellationToken);

        await cart.RemoveAsync(_contentLocator,
                               _forexConverter,
                               _priceCalculator,
                               _lookups,
                               req.Model.GivingType,
                               req.Model.Index.GetValueOrThrow());

        await _repository.UpdateAsync(cart);

        return cart.RevisionId;
    }

    public async Task<RevisionId> Handle(RemoveUpsellFromCartCommand req, CancellationToken cancellationToken) {
        var cart = await _cartAccessor.GetAsync(cancellationToken);
        var upsellOfferContent = req.UpsellOfferId.Run(_contentLocator.ById<UpsellOfferContent>, true);
        
        await cart.RemoveUpsellAsync(_contentLocator,
                                     _forexConverter,
                                     _priceCalculator,
                                     _lookups,
                                     upsellOfferContent.GivingType,
                                     req.UpsellOfferId.Value);
        
        await _repository.UpdateAsync(cart);

        return cart.RevisionId;
    }
}