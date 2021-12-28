using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Cart.Database;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Json;
using NodaTime;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Scoping;

namespace N3O.Umbraco.Giving.Cart;

public class CartRepository : ICartRepository {
    private readonly IScopeProvider _scopeProvider;
    private readonly IJsonProvider _jsonProvider;
    private readonly ICartValidator _cartValidator;
    private readonly IClock _clock;

    public CartRepository(IScopeProvider scopeProvider,
                          IJsonProvider jsonProvider,
                          ICartValidator cartValidator,
                          IClock clock) {
        _scopeProvider = scopeProvider;
        _jsonProvider = jsonProvider;
        _cartValidator = cartValidator;
        _clock = clock;
    }


    public Task ClearAsync(Guid cartId) {
        using (var scope = _scopeProvider.CreateScope()) {
            scope.Database.Delete<CartTable>($@"DELETE * FROM {CartConstants.Tables.Carts} WHERE Id = @0", cartId);
            
            scope.Complete();

            return Task.CompletedTask;
        }
    }

    public async Task<DonationCart> GetOrCreateCartAsync(Guid cartId,
                                                         Currency currency,
                                                         CancellationToken cancellationToken = default) {
        using (var scope = _scopeProvider.CreateScope()) {
            var row = await scope.Database.SingleOrDefaultAsync<CartTable>($@"SELECT * FROM {CartConstants.Tables.Carts}
                                                                              WHERE Id = @0", cartId);

            var cart = row?.Json.IfNotNull(x => _jsonProvider.DeserializeObject<DonationCart>(x));

            if (cart == null || !_cartValidator.IsValid(currency, cart)) {
                cart = DonationCart.Create(_clock.GetCurrentInstant(), currency);
            }
            
            return cart;
        }
    }

    public Task SaveCartAsync(DonationCart cart, CancellationToken cancellationToken = default) {
        using (var scope = _scopeProvider.CreateScope()) {
            var row = new CartTable();
            row.Id = cart.Id;
            row.Json = _jsonProvider.SerializeObject(cart);

            scope.Database.Save(row);
            
            scope.Complete();

            return Task.CompletedTask;
        }
    }
}