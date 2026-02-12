using N3O.Umbraco.Cloud.Platforms.Clients;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms;

public interface ICampaignAccessor {
    Task<PublishedCampaign> GetAsync(IPublishedContent content, CancellationToken cancellationToken = default);
}
