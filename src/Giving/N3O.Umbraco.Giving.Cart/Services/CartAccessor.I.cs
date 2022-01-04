using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart {
    public interface ICartAccessor {
        Task<Entities.Cart> GetAsync(CancellationToken cancellationToken = default);
    }
}