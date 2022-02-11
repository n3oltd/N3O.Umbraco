using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart {
    public interface ICartAccessor {
        Entities.Cart Get();
        Task<Entities.Cart> GetAsync(CancellationToken cancellationToken = default);
    }
}