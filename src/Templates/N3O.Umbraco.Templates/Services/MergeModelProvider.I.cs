using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Templates;

public interface IMergeModelProvider {
    bool IsProviderFor(IPublishedContent content);
    Task<object> GetAsync(IPublishedContent content, CancellationToken cancellationToken = default);
    string Key { get; }
}