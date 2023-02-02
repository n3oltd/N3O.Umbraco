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
    private readonly Lazy<ICurrencyAccessor> _currencyAccessor;

    public AddToCartHandlers(ICartAccessor cartAccessor,
                             IRepository<Entities.Cart> repository,
                             Lazy<IContentLocator> contentLocator,
                             Lazy<IForexConverter> forexConverter,
                             Lazy<ICurrencyAccessor> currencyAccessor) {
        _cartAccessor = cartAccessor;
        _repository = repository;
        _contentLocator = contentLocator;
        _forexConverter = forexConverter;
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
        var allocation = await upsellContent.GetAllocationAsync(_forexConverter.Value, currency);

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