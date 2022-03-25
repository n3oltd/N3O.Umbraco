using N3O.Umbraco.Context;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Locks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart {
    public class CartAccessor : ICartAccessor {
        private readonly ICartIdAccessor _cartIdAccessor;
        private readonly IRepository<Entities.Cart> _repository;
        private readonly ICurrencyAccessor _currencyAccessor;
        private readonly Lazy<ICartValidator> _cartValidator;
        private readonly ILocker _locker;

        public CartAccessor(ICartIdAccessor cartIdAccessor,
                            IRepository<Entities.Cart> repository,
                            ICurrencyAccessor currencyAccessor,
                            Lazy<ICartValidator> cartValidator,
                            ILocker locker) {
            _cartIdAccessor = cartIdAccessor;
            _repository = repository;
            _currencyAccessor = currencyAccessor;
            _cartValidator = cartValidator;
            _locker = locker;
        }

        public Entities.Cart Get() {
            return GetAsync().GetAwaiter().GetResult();
        }

        public async Task<Entities.Cart> GetAsync(CancellationToken cancellationToken = default) {
            var cartId = _cartIdAccessor.GetId();
            
            using (await _locker.LockAsync(cartId.ToString())) {
                var currency = _currencyAccessor.GetCurrency();
                var cart = await _repository.GetAsync(cartId);

                if (cart == null) {
                    cart = Entities.Cart.Create(cartId, currency);

                    await _repository.InsertAsync(cart);
                } else {
                    if (!_cartValidator.Value.IsValid(currency, cart)) {
                        cart.Reset(currency);
                    }
                }

                return cart;
            }
        }
    }
}