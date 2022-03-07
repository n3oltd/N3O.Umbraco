using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Handlers {
    public class ClearCartHandler : IRequestHandler<ClearCartCommand, ClearCartReq, None> {
        private readonly IRepository<Entities.Cart> _repository;

        public ClearCartHandler(IRepository<Entities.Cart> repository) {
            _repository = repository;
        }
    
        public async Task<None> Handle(ClearCartCommand req, CancellationToken cancellationToken) {
            var cart = await req.CartId.RunAsync(_repository.GetAsync, true);

            cart.RemoveAll(req.Model.GivingType);

            await _repository.UpdateAsync(cart, RevisionBehaviour.Unchanged);

            return None.Empty;
        }
    }
}