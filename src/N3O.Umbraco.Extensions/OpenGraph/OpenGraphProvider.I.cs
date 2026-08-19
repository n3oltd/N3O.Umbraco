using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.OpenGraph;

public interface IOpenGraphProvider {
    Task AddOpenGraphAsync(IOpenGraphBuilder builder, IPublishedContent page);
    bool IsProviderFor(IPublishedContent page);
}
