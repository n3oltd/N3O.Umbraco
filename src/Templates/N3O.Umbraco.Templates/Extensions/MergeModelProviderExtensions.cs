using Humanizer;
using N3O.Umbraco.Extensions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Templates.Extensions;

public static class MergeModelProviderExtensions {
    public static async Task<IReadOnlyDictionary<string, object>> GetMergeModelsAsync(this IEnumerable<IMergeModelProvider> mergeModelProviders,
                                                                                      IPublishedContent content,
                                                                                      ConcurrentDictionary<IPublishedContent, IReadOnlyDictionary<string, object>> cache) {
        return await cache.GetOrAddAtomicAsync<IPublishedContent, IReadOnlyDictionary<string, object>>(content, async () => {
            var mergeModels = new Dictionary<string, object>();

            foreach (var provider in mergeModelProviders.ApplyAttributeOrdering()) {
                if (await provider.IsProviderForAsync(content)) {
                    mergeModels[provider.Key.Camelize()] = await provider.GetAsync(content);   
                }
            }

            return mergeModels;
        });
    }
}