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
        private readonly Lazy<ICurrencyAccessor> _currencyAccessor;
        private readonly ILock _lock;

        public CartAccessor(ICartIdAccessor cartIdAccessor,
                            IRepository<Entities.Cart> repository,
                            Lazy<ICurrencyAccessor> currencyAccessor,
                            ILock @lock) {
            _cartIdAccessor = cartIdAccessor;
            _repository = repository;
            _currencyAccessor = currencyAccessor;
            _lock = @lock;
        }
    
        public async Task<Entities.Cart> GetAsync(CancellationToken cancellationToken = default) {
            var cartId = _cartIdAccessor.GetCartId();
            
            var result = await _lock.LockAsync(cartId.ToString(), async () => {
                var cart = await _repository.GetAsync(cartId, cancellationToken);

                if (cart == null) {
                    var currency = _currencyAccessor.Value.GetCurrency();
                    
                    cart = Entities.Cart.Create(cartId, currency);

                    await _repository.InsertAsync(cart, cancellationToken);
                }

                return cart;
            });

            return result;
        }
    }
}