using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Mediator;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Handlers;

public class AddToCartHandlers :
    IRequestHandler<AddToCartCommand, AddToCartReq, RevisionId>,
    IRequestHandler<AddUpsellToCartCommand, None, RevisionId> {
    private readonly ICartAccessor _cartAccessor;
    private readonly IRepository<Entities.Cart> _repository;
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly Lazy<IForexConverter> _forexConverter;
    private readonly Lazy<IPriceCalculator> _priceCalculator;
    private readonly Lazy<ICurrencyAccessor> _currencyAccessor;

    public AddToCartHandlers(ICartAccessor cartAccessor,
                             IRepository<Entities.Cart> repository,
                             Lazy<IContentLocator> contentLocator,
                             Lazy<IForexConverter> forexConverter,
                             Lazy<IPriceCalculator> priceCalculator,
                             Lazy<ICurrencyAccessor> currencyAccessor) {
        _cartAccessor = cartAccessor;
        _repository = repository;
        _contentLocator = contentLocator;
        _forexConverter = forexConverter;
        _priceCalculator = priceCalculator;
        _currencyAccessor = currencyAccessor;
    }

    public async Task<RevisionId> Handle(AddToCartCommand req, CancellationToken cancellationToken) {
        var revisionId = await AddToCartAsync(req.Model.GivingType,
                                              req.Model.Allocation,
                                              req.Model.Quantity.GetValueOrThrow(),
                                              cancellationToken);

        return revisionId;
    }

    public async Task<RevisionId> Handle(AddUpsellToCartCommand req, CancellationToken cancellationToken) {
        var upsellContent = req.UpsellId.Run(_contentLocator.Value.ById<UpsellContent>, true);

        var currency = _currencyAccessor.Value.GetCurrency();
        
        // Add a contentment lookup called UpsellPricingMode with values
        // DonationItem, Fixed, Any, PriceHandles, Custom
        // Validate that if donation item has pricing, then mode must be DonationItem
        // if not fixed, fixed amount must be blank and vice versa etc.
        // If PriceHandles then have a property group for titled price handles
        // If set to Custom then in UpsellContent look for a specific class that implements an interface and has the methods, e.g.
        
        
        /*
         *      General
         *          Name:
         *          Item:
         *          Pricing Mode: Handles
         *
         *      Fixed
         *          Amount:
         *
         *      Price Handles:
         *          Amount
         *          Locked
         *          (compose same way donation items)
         */
        
        /*
         * public class OneNationUpsellPricing : ICustomUpsellPricing {
         *      Money GetAmount(UpsellContent content) {
         *            Do any formula to calculate this
         *      }
         * 
         * }
         *
         * UpsellContent -> OurAssemblies.GetTypes(t => t.COncrete() && t.parameterlessconstructor() && t.implementinterface<ICustomUpsellPricing>())
         * .Single() -> you can call the methods
         */
        
        
        
        
        var allocation = await upsellContent.GetAllocationAsync(_forexConverter.Value,
                                                                _priceCalculator.Value,
                                                                currency);

        var revisionId = await AddToCartAsync(upsellContent.GivingType, allocation, 1, cancellationToken);

        return revisionId;
    }

    private async Task<RevisionId> AddToCartAsync(GivingType givingType,
                                                  IAllocation allocation,
                                                  int quantity,
                                                  CancellationToken cancellationToken) {
        var cart = await _cartAccessor.GetAsync(cancellationToken);

        cart.Add(givingType, allocation, quantity);

        await _repository.UpdateAsync(cart);

        return cart.RevisionId;
    }
}