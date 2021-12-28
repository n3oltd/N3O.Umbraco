using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Mediator;
using NodaTime;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Handlers;

public class RemoveFromCartHandler : IRequestHandler<RemoveFromCartCommand, None, None> {
    private readonly ICartAccessor _cartAccessor;
    private readonly ICartRepository _cartRepository;
    private readonly IClock _clock;

    public RemoveFromCartHandler(ICartAccessor cartAccessor, ICartRepository cartRepository, IClock clock) {
        _cartAccessor = cartAccessor;
        _cartRepository = cartRepository;
        _clock = clock;
    }
    
    public async Task<None> Handle(RemoveFromCartCommand req, CancellationToken cancellationToken) {
        var cart = await _cartAccessor.GetAsync(cancellationToken);

        var regularContents = RemoveAllocation(cart.Currency, cart.Regular, req.AllocationIndex.Value);
        var singleContents = RemoveAllocation(cart.Currency, cart.Single, req.AllocationIndex.Value);

        cart = new DonationCart(cart.Id, _clock.GetCurrentInstant(), cart.Currency, singleContents, regularContents);

        await _cartRepository.SaveCartAsync(cart, cancellationToken);
        
        return None.Empty;
    }

    private CartContents RemoveAllocation(Currency currency, CartContents cartContents, int allocationIndex) {
        var allocations = cartContents.Allocations.ExceptAt(allocationIndex);

        return new CartContents(currency, cartContents.Type, allocations);
    }
}