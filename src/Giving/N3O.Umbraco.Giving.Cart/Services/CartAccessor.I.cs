using N3O.Umbraco.Giving.Cart.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart;

public interface ICartAccessor {
    Task<DonationCart> GetAsync(CancellationToken cancellationToken = default);
}