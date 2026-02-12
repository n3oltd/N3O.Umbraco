using N3O.Umbraco.Entities;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms;

public interface ICampaignIdProvider {
    Task<EntityId> GetIdAsync(IPublishedContent content, CancellationToken cancellationToken = default);
}