using N3O.Umbraco.Newsletters.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Newsletters;

public interface INewslettersClient {
    Task<SubscribeResult> SubscribeAsync(IContact contact, CancellationToken cancellationToken = default);
}
