using N3O.Umbraco.Context;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Cart;
using N3O.Umbraco.Locks;
using N3O.Umbraco.References;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Checkout {
    public class CheckoutAccessor : ICheckoutAccessor {
        private readonly ICheckoutIdAccessor _checkoutIdAccessor;
        private readonly ILock _lock;
        private readonly IRepository<Entities.Checkout> _repository;
        private readonly Lazy<ICartAccessor> _cartAccessor;
        private readonly Lazy<ICounters> _counters;
        private readonly Lazy<IRemoteIpAddressAccessor> _remoteIpAddressAccessor;

        public CheckoutAccessor(ICheckoutIdAccessor checkoutIdAccessor,
                                ILock @lock,
                                IRepository<Entities.Checkout> repository,
                                Lazy<ICartAccessor> cartAccessor,
                                Lazy<ICounters> counters,
                                Lazy<IRemoteIpAddressAccessor> remoteIpAddressAccessor) {
            _checkoutIdAccessor = checkoutIdAccessor;
            _lock = @lock;
            _repository = repository;
            _cartAccessor = cartAccessor;
            _counters = counters;
            _remoteIpAddressAccessor = remoteIpAddressAccessor;
        }

        public Entities.Checkout Get() {
            return GetAsync().GetAwaiter().GetResult();
        }

        public async Task<Entities.Checkout> GetAsync(CancellationToken cancellationToken = default) {
            var checkoutId = _checkoutIdAccessor.GetId();

            var checkout = await _repository.GetAsync(checkoutId);

            return checkout;
        }
        
        public async Task<Entities.Checkout> GetOrCreateAsync(CancellationToken cancellationToken) {
            var checkoutId = _checkoutIdAccessor.GetId();
            
            var result = await _lock.LockAsync(checkoutId.ToString(), async () => {
                var checkout = await _repository.GetAsync(checkoutId);

                if (checkout == null) {
                    var cart = await _cartAccessor.Value.GetAsync(cancellationToken);

                    if (!cart.IsEmpty()) {
                        checkout = await Entities.Checkout.CreateAsync(_counters.Value,
                                                                       _remoteIpAddressAccessor.Value,
                                                                       checkoutId,
                                                                       cart);

                        await _repository.InsertAsync(checkout);    
                    }
                }

                return checkout;
            });

            return result;
        }
    }
}