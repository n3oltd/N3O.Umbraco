using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Templates;

public abstract class MergeModelProvider<TModel> : IMergeModelProvider {
    public virtual bool IsProviderFor(IPublishedContent content) => true;

    public async Task<object> GetAsync(IPublishedContent content, CancellationToken cancellationToken = default) {
        return await GetModelAsync(content, cancellationToken);
    }
    
    public abstract string Key { get; }

    protected abstract Task<TModel> GetModelAsync(IPublishedContent content, CancellationToken cancellationToken);
}

public abstract class MergeModelProvider<TContent, TModel> : IMergeModelProvider where TContent : IPublishedContent {
    public virtual bool IsProviderFor(IPublishedContent content) {
        return content.GetType().IsAssignableTo(typeof(TContent));
    }

    public async Task<object> GetAsync(IPublishedContent content, CancellationToken cancellationToken = default) {
        return await GetModelAsync((TContent) content, cancellationToken);
    }
    
    public abstract string Key { get; }

    protected abstract Task<TModel> GetModelAsync(TContent content, CancellationToken cancellationToken);
}