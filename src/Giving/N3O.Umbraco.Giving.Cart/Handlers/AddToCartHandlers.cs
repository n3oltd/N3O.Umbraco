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
using System.Collections.Generic;
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
    private readonly IEnumerable<IAllocationExtensionRequestBinder> _extensionBinders;

    public AddToCartHandlers(ICartAccessor cartAccessor,
                             IRepository<Entities.Cart> repository,
                             IContentLocator contentLocator,
                             Lazy<ICurrencyAccessor> currencyAccessor,
                             IForexConverter forexConverter,
                             IPriceCalculator priceCalculator,
                             IEnumerable<IAllocationExtensionRequestBinder> extensionBinders) {
        _cartAccessor = cartAccessor;
        _repository = repository;
        _contentLocator = contentLocator;
        _currencyAccessor = currencyAccessor;
        _forexConverter = forexConverter;
        _priceCalculator = priceCalculator;
        _extensionBinders = extensionBinders;
    }

    public async Task<RevisionId> Handle(AddToCartCommand req, CancellationToken cancellationToken) {
        var allocation = GetAllocationData(req.Model.Allocation);
        
        var revisionId = await AddToCartAsync(req.Model.GivingType,
                                              allocation,
                                              req.Model.Quantity.GetValueOrThrow(),
                                              cancellationToken);

        return revisionId;
    }

    public async Task<RevisionId> Handle(AddUpsellToCartCommand req, CancellationToken cancellationToken) {
        var cart = await _cartAccessor.GetAsync(cancellationToken);
        var upsellOfferContent = req.UpsellOfferId.Run(_contentLocator.ById<UpsellOfferContent>, true);
        
        if (cart.ContainsUpsell(req.UpsellOfferId.Value) && !upsellOfferContent.AllowMultiple) {
            throw new Exception($"Offer {req.UpsellOfferId.Value} already exists in this cart");
        }

        var currency = _currencyAccessor.Value.GetCurrency();
        
        var allocation = await upsellOfferContent.ToAllocationAsync(_forexConverter,
                                                                    _priceCalculator,
                                                                    currency,
                                                                    req.Model.Amount,
                                                                    upsellOfferContent.GivingType,
                                                                    cart.GetTotalExcludingUpsells(upsellOfferContent.GivingType));

        var revisionId = await AddToCartAsync(upsellOfferContent.GivingType, allocation, 1, cancellationToken);

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
    
    private IAllocation GetAllocationData(AllocationReq req) {
        var extensionData = req.Extensions?.BindAll(_extensionBinders);
        
        var allocation = new Allocation(req, extensionData);

        return allocation;
    }
}