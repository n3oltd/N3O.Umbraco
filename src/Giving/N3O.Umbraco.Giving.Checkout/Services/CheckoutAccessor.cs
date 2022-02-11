using N3O.Umbraco.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Checkout {
    public class CheckoutAccessor : ICheckoutAccessor {
        private readonly ICheckoutIdAccessor _checkoutIdAccessor;
        private readonly IRepository<Entities.Checkout> _repository;

        public CheckoutAccessor(ICheckoutIdAccessor checkoutIdAccessor, IRepository<Entities.Checkout> repository) {
            _checkoutIdAccessor = checkoutIdAccessor;
            _repository = repository;
        }

        public Entities.Checkout Get() {
            return GetAsync().GetAwaiter().GetResult();
        }

        public async Task<Entities.Checkout> GetAsync(CancellationToken cancellationToken = default) {
            var checkoutId = _checkoutIdAccessor.GetCheckoutId();

            var checkout = await _repository.GetAsync(checkoutId, cancellationToken);

            return checkout;
        }
    }
}