using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Handlers {
    public class AddToCartHandler : IRequestHandler<AddToCartCommand, AddToCartReq, None> {
        private readonly ICartAccessor _cartAccessor;
        private readonly IRepository<Entities.Cart> _repository;

        public AddToCartHandler(ICartAccessor cartAccessor, IRepository<Entities.Cart> repository) {
            _cartAccessor = cartAccessor;
            _repository = repository;
        }
    
        public async Task<None> Handle(AddToCartCommand req, CancellationToken cancellationToken) {
            var cart = await _cartAccessor.GetAsync(cancellationToken);
            
            cart.Add(req.Model.GivingType, req.Model.Allocation, req.Model.Quantity.GetValueOrThrow());

            await _repository.UpdateAsync(cart, cancellationToken);
        
            return None.Empty;
        }
    }
}