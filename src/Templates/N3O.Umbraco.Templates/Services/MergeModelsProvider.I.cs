using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Templates;

public interface IMergeModelsProvider {
    Task<bool> IsProviderForAsync(IPublishedContent content);
    Task<IReadOnlyDictionary<string, object>> GetModelsAsync(IPublishedContent content,
                                                             CancellationToken cancellationToken = default);
}