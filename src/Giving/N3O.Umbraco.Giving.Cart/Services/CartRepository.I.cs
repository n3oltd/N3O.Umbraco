using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Cart.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart {
    public interface ICartRepository {
        Task ClearAsync(Guid cartId);
    
        Task<DonationCart> GetOrCreateCartAsync(Guid cartId,
                                                Currency currency,
                                                CancellationToken cancellationToken = default);
    
        Task SaveCartAsync(DonationCart cart, CancellationToken cancellationToken = default);
    }
}