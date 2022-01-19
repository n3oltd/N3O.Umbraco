using N3O.Umbraco.Counters;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Cart;
using N3O.Umbraco.Locks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Checkout {
    public class CheckoutAccessor : ICheckoutAccessor {
        private readonly ICheckoutIdAccessor _checkoutIdAccessor;
        private readonly IRepository<Entities.Checkout> _repository;
        private readonly Lazy<ICartAccessor> _cartAccessor;
        private readonly Lazy<ICounters> _counters;
        private readonly ILock _lock;

        public CheckoutAccessor(ICheckoutIdAccessor checkoutIdAccessor,
                                IRepository<Entities.Checkout> repository,
                                Lazy<ICartAccessor> cartAccessor,
                                Lazy<ICounters> counters,
                                ILock @lock) {
            _checkoutIdAccessor = checkoutIdAccessor;
            _repository = repository;
            _cartAccessor = cartAccessor;
            _counters = counters;
            _lock = @lock;
        }
    
        public async Task<Entities.Checkout> GetAsync(CancellationToken cancellationToken = default) {
            var checkoutId = _checkoutIdAccessor.GetCheckoutId();
            
            var result = await _lock.LockAsync(checkoutId.ToString(), async () => {
                var checkout = await _repository.GetAsync(checkoutId, cancellationToken);

                if (checkout == null) {
                    var cart = await _cartAccessor.Value.GetAsync(cancellationToken);
                    
                    checkout = await Entities.Checkout.CreateAsync(_counters.Value, checkoutId, cart);

                    await _repository.InsertAsync(checkout, cancellationToken);
                }

                return checkout;
            });

            return result;
        }
    }
}