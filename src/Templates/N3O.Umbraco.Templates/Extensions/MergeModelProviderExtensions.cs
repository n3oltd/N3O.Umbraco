using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Templates.Extensions;

public static class MergeModelProviderExtensions {
    public static async Task<IReadOnlyDictionary<string, object>> GetMergeModelsAsync(this IEnumerable<IMergeModelsProvider> mergeModelsProviders,
                                                                                      ILogger logger,
                                                                                      IPublishedContent content,
                                                                                      ConcurrentDictionary<IPublishedContent, IReadOnlyDictionary<string, object>> cache,
                                                                                      CancellationToken cancellationToken = default) {
        return await cache.GetOrAddAtomicAsync<IPublishedContent, IReadOnlyDictionary<string, object>>(content, async () => {
            var mergeModels = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var provider in mergeModelsProviders.ApplyAttributeOrdering()) {
                try {
                    if (await provider.IsProviderForAsync(content)) {
                        var models = await provider.GetModelsAsync(content, cancellationToken);

                        foreach (var (key, data) in models) {
                            mergeModels[key] = data;
                        }
                    }
                } catch (Exception ex) {
                    logger.LogError(ex, "Failed to get model for {Provider}", provider.GetType().Name);
                }
            }

            return mergeModels;
        });
    }
}