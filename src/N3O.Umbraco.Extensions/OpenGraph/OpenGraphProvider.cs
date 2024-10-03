using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.OpenGraph;

public abstract class OpenGraphProvider<T> : IOpenGraphProvider where T : IPublishedContent {
    public void AddOpenGraph(IOpenGraphBuilder builder, IPublishedContent page) {
        AddOpenGraph(builder, (T) page);
    }

    public virtual bool IsProviderFor(IPublishedContent page) {
        return page.GetType().IsAssignableTo(typeof(T));
    }

    protected abstract void AddOpenGraph(IOpenGraphBuilder builder, T page);
}
