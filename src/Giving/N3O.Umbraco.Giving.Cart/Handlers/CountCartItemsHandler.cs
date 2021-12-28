using N3O.Umbraco.Giving.Cart.Queries;
using N3O.Umbraco.Mediator;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Handlers;

public class CountCartItemsHandler : IRequestHandler<CountCartItemsQuery, None, int> {
    private readonly ICartAccessor _cartAccessor;

    public CountCartItemsHandler(ICartAccessor cartAccessor) {
        _cartAccessor = cartAccessor;
    }
    
    public async Task<int> Handle(CountCartItemsQuery req, CancellationToken cancellationToken) {
        var cart = await _cartAccessor.GetAsync(cancellationToken);

        var count = cart.Single.Allocations.Count() + cart.Regular.Allocations.Count();

        return count;
    }
}