using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Mediator;
using NodaTime;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Handlers;

public class AddToCartHandler : IRequestHandler<AddToCartCommand, AddToCartReq, None> {
    private readonly ICartAccessor _cartAccessor;
    private readonly ICartRepository _cartRepository;
    private readonly IClock _clock;

    public AddToCartHandler(ICartAccessor cartAccessor, ICartRepository cartRepository, IClock clock) {
        _cartAccessor = cartAccessor;
        _cartRepository = cartRepository;
        _clock = clock;
    }
    
    public async Task<None> Handle(AddToCartCommand req, CancellationToken cancellationToken) {
        var cart = await _cartAccessor.GetAsync(cancellationToken);

        var regularContents = cart.Regular;
        var singleContents = cart.Single;

        if (req.Model.DonationType == DonationTypes.Single) {
            regularContents = AddAllocation(cart.Currency, regularContents, req.Model.Allocation);
        } else if (req.Model.DonationType == DonationTypes.Regular) {
            singleContents = AddAllocation(cart.Currency, singleContents, req.Model.Allocation);
        } else {
            throw UnrecognisedValueException.For(req.Model.DonationType);
        }

        cart = new DonationCart(cart.Id, _clock.GetCurrentInstant(), cart.Currency, singleContents, regularContents);

        await _cartRepository.SaveCartAsync(cart, cancellationToken);
        
        return None.Empty;
    }

    private CartContents AddAllocation(Currency currency, CartContents cartContents, IAllocation allocation) {
        var allocations = cartContents.Allocations.Concat(new Allocation(allocation)).ToList();

        return new CartContents(currency, cartContents.Type, allocations);
    }
}