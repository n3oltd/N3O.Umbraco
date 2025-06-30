using N3O.Umbraco.Cloud.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud;

public interface IOrganizationInfoAccessor {
    Task<IOrganizationInfo> GetOrganizationInfoAsync(CancellationToken cancellationToken = default);
}