using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Checkout {
    public interface ICheckoutAccessor {
        Task<Entities.Checkout> GetCheckoutAsync(CancellationToken cancellationToken);
    }
}