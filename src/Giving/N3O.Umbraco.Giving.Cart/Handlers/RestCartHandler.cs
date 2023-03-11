using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Handlers;

public class ResetCartHandler : IRequestHandler<ResetCartCommand, None, None>{
    private readonly IRepository<Entities.Cart> _repository;
    private readonly ICartAccessor _cartAccessor;

    public ResetCartHandler(IRepository<Entities.Cart> repository,
                            ICartAccessor cartAccessor) {
        _repository = repository;
        _cartAccessor = cartAccessor;
    }

    public async Task<None> Handle(ResetCartCommand req, CancellationToken cancellationToken) {
        var cart = await _cartAccessor.GetAsync(cancellationToken);

        cart.Reset();

        await _repository.UpdateAsync(cart);

        return None.Empty;
    }
}
