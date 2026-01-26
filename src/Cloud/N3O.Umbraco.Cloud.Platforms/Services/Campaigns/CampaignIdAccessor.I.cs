using N3O.Umbraco.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms;

public interface ICampaignIdAccessor {
    Task<EntityId> GetIdAsync(CancellationToken cancellationToken = default);
}
