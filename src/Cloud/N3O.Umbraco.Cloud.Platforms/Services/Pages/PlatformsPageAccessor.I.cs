using N3O.Umbraco.Cloud.Platforms.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms;

public interface IPlatformsPageAccessor {
    Task<GetPageResult> GetAsync(CancellationToken cancellationToken = default);
}