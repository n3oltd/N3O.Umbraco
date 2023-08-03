using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Extensions;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Mediator;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Handlers;

public class AddToCartHandlers :
    IRequestHandler<AddToCartCommand, AddToCartReq, RevisionId>,
    IRequestHandler<AddUpsellToCartCommand, AddUpsellToCartReq, RevisionId> {
    private readonly ICartAccessor _cartAccessor;
    private readonly IRepository<Entities.Cart> _repository;
    private readonly IContentLocator _contentLocator;
    private readonly Lazy<ICurrencyAccessor> _currencyAccessor;
    private readonly IForexConverter _forexConverter;
    private readonly IPriceCalculator _priceCalculator;

    public AddToCartHandlers(ICartAccessor cartAccessor,
                             IRepository<Entities.Cart> repository,
                             IContentLocator contentLocator,
                             Lazy<ICurrencyAccessor> currencyAccessor,
                             IForexConverter forexConverter,
                             IPriceCalculator priceCalculator) {
        _cartAccessor = cartAccessor;
        _repository = repository;
        _contentLocator = contentLocator;
        _currencyAccessor = currencyAccessor;
        _forexConverter = forexConverter;
        _priceCalculator = priceCalculator;
    }

    public async Task<RevisionId> Handle(AddToCartCommand req, CancellationToken cancellationToken) {
        var revisionId = await AddToCartAsync(req.Model.GivingType,
                                              req.Model.Allocation,
                                              req.Model.Quantity.GetValueOrThrow(),
                                              cancellationToken);

        return revisionId;
    }

    public async Task<RevisionId> Handle(AddUpsellToCartCommand req, CancellationToken cancellationToken) {
        var cart = await _cartAccessor.GetAsync(cancellationToken);
        var upsellContent = req.UpsellId.Run(_contentLocator.ById<UpsellOfferContent>, true);
        
        if (cart.ContainsUpsell(upsellContent.GivingType, req.UpsellId.Value) && !upsellContent.AllowMultiple) {
            throw new Exception($"Upsell offer {req.UpsellId.Value} is already added to cart and is not allowed multiple time");
        }

        var currency = _currencyAccessor.Value.GetCurrency();
        
        var allocation = await upsellContent.ToAllocationAsync(_forexConverter,
                                                               _priceCalculator,
                                                               currency,
                                                               req.Model.Amount,
                                                               upsellContent.GivingType,
                                                               cart.GetTotalExcludingUpsells(upsellContent.GivingType));

        var revisionId = await AddToCartAsync(upsellContent.GivingType, allocation, 1, cancellationToken);

        return revisionId;
    }

    private async Task<RevisionId> AddToCartAsync(GivingType givingType,
                                                  IAllocation allocation,
                                                  int quantity,
                                                  CancellationToken cancellationToken) {
        var cart = await _cartAccessor.GetAsync(cancellationToken);

        await cart.AddAsync(_contentLocator, _forexConverter, _priceCalculator, givingType, allocation, quantity);

        await _repository.UpdateAsync(cart);

        return cart.RevisionId;
    }
}