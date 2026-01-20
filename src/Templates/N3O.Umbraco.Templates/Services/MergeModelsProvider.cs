using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Templates;

public abstract class MergeModelsProvider : IMergeModelsProvider {
    public virtual Task<bool> IsProviderForAsync(IPublishedContent content) => Task.FromResult(true);

    public async Task<IReadOnlyDictionary<string, object>> GetModelsAsync(IPublishedContent content,
                                                                          CancellationToken cancellationToken = default) {
        var mergeModels = new Dictionary<string, object>();

        await PopulateModelsAsync(content, mergeModels, cancellationToken);
        
        return mergeModels;
    }

    protected abstract Task PopulateModelsAsync(IPublishedContent content,
                                                Dictionary<string, object> mergeModels,
                                                CancellationToken cancellationToken = default);
}

public abstract class MergeModelsProvider<TContent> : MergeModelsProvider where TContent : IPublishedContent {
    public override Task<bool> IsProviderForAsync(IPublishedContent content) {
        return Task.FromResult(content.GetType().IsAssignableTo(typeof(TContent)));
    }
}