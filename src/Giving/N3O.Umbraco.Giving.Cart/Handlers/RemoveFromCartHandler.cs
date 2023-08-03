using N3O.Umbraco.Content;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Handlers;

public class RemoveFromCartHandler :
    IRequestHandler<RemoveFromCartCommand, RemoveFromCartReq, RevisionId>,
    IRequestHandler<RemoveUpsellFromCartCommand, None, RevisionId> {
    private readonly IContentLocator _contentLocator;
    private readonly IForexConverter _forexConverter;
    private readonly IPriceCalculator _priceCalculator;
    private readonly ICartAccessor _cartAccessor;
    private readonly IRepository<Entities.Cart> _repository;

    public RemoveFromCartHandler(IContentLocator contentLocator,
                                 IForexConverter forexConverter,
                                 IPriceCalculator priceCalculator,
                                 ICartAccessor cartAccessor,
                                 IRepository<Entities.Cart> repository) {
        _contentLocator = contentLocator;
        _forexConverter = forexConverter;
        _priceCalculator = priceCalculator;
        _cartAccessor = cartAccessor;
        _repository = repository;
    }

    public async Task<RevisionId> Handle(RemoveFromCartCommand req, CancellationToken cancellationToken) {
        var cart = await _cartAccessor.GetAsync(cancellationToken);

        await cart.RemoveAsync(_contentLocator,
                               _forexConverter,
                               _priceCalculator,
                               req.Model.GivingType,
                               req.Model.Index.GetValueOrThrow());

        await _repository.UpdateAsync(cart);

        return cart.RevisionId;
    }

    public async Task<RevisionId> Handle(RemoveUpsellFromCartCommand req, CancellationToken cancellationToken) {
        var cart = await _cartAccessor.GetAsync(cancellationToken);
        
        var upsellContent = req.UpsellId.Run(_contentLocator.ById<UpsellOfferContent>, true);
        
        await cart.RemoveUpsellAsync(_contentLocator,
                                     _forexConverter,
                                     _priceCalculator,
                                     upsellContent.GivingType,
                                     req.UpsellId.Value);
        
        await _repository.UpdateAsync(cart);

        return cart.RevisionId;
    }
}