using N3O.Umbraco.Context;
using N3O.Umbraco.Giving.Cart.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart {
    public class CartAccessor : ICartAccessor {
        private readonly ICartIdAccessor _cartIdAccessor;
        private readonly ICurrencyAccessor _currencyAccessor;
        private readonly ICartRepository _cartRepository;

        public CartAccessor(ICartIdAccessor cartIdAccessor,
                            ICurrencyAccessor currencyAccessor,
                            ICartRepository cartRepository) {
            _cartIdAccessor = cartIdAccessor;
            _currencyAccessor = currencyAccessor;
            _cartRepository = cartRepository;
        }
    
        public async Task<DonationCart> GetAsync(CancellationToken cancellationToken = default) {
            var cartId = _cartIdAccessor.GetCartId();
            var currency = _currencyAccessor.GetCurrency();
            var cart = await _cartRepository.GetOrCreateCartAsync(cartId, currency, cancellationToken);

            return cart;
        }
    }
}