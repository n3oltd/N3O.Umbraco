using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Checkout;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Checkout {
    public class CheckoutAccessor : ICheckoutAccessor {
        private readonly ICheckoutIdAccessor _checkoutIdAccessor;
        private readonly IRepository<Entities.Checkout> _repository;

        public CheckoutAccessor(ICheckoutIdAccessor checkoutIdAccessor,
                                IRepository<Entities.Checkout> repository) {
            _checkoutIdAccessor = checkoutIdAccessor;
            _repository = repository;
        }

        public async Task<Entities.Checkout> GetCheckoutAsync(CancellationToken cancellationToken) {
            var checkoutId = _checkoutIdAccessor.GetCheckoutId();

            var checkout = await _repository.GetAsync(checkoutId, cancellationToken);

            return checkout;
        }
    }
}