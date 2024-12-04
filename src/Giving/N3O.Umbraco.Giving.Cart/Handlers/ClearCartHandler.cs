using N3O.Umbraco.Content;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Allocations;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Handlers;

public class ClearCartHandler : IRequestHandler<ClearCartCommand, ClearCartReq, None> {
    private readonly IContentLocator _contentLocator;
    private readonly IForexConverter _forexConverter;
    private readonly IPriceCalculator _priceCalculator;
    private readonly IRepository<Entities.Cart> _repository;

    public ClearCartHandler(IContentLocator contentLocator,
                            IForexConverter forexConverter,
                            IPriceCalculator priceCalculator,
                            IRepository<Entities.Cart> repository) {
        _contentLocator = contentLocator;
        _forexConverter = forexConverter;
        _priceCalculator = priceCalculator;
        _repository = repository;
    }

    public async Task<None> Handle(ClearCartCommand req, CancellationToken cancellationToken) {
        var cart = await req.CartId.RunAsync(_repository.GetAsync, true, cancellationToken);

        await cart.RemoveAllAsync(_contentLocator, _forexConverter, _priceCalculator, req.Model.GivingType);

        await _repository.UpdateAsync(cart, RevisionBehaviour.Unchanged);

        return None.Empty;
    }
}
