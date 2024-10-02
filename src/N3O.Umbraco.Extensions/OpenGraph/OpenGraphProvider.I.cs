using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.OpenGraph;

public interface IOpenGraphProvider {
    void AddOpenGraph(IOpenGraphBuilder builder, IPublishedContent page);
    bool IsProviderFor(IPublishedContent page);
}
