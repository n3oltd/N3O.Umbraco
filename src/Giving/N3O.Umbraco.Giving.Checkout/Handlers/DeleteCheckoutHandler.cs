using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Checkout.Commands;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Checkout.Handlers {
    public class DeleteCheckoutHandler : IRequestHandler<DeleteCheckoutCommand, Entities.Checkout, None> {
        private readonly IRepository<Entities.Checkout> _repository;

        public DeleteCheckoutHandler(IRepository<Entities.Checkout> repository) {
            _repository = repository;
        }
        
        public async Task<None> Handle(DeleteCheckoutCommand req, CancellationToken cancellationToken) {
            await _repository.DeleteAsync(req.Model, cancellationToken);

            return None.Empty;
        }
    }
}