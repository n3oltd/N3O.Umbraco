using N3O.Umbraco.Analytics.Models;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Analytics {
    public interface IGAEventsProvider {
        bool IsProviderFor(IPublishedContent page);
        Task RunAsync(IPublishedContent page, GTag gTag, CancellationToken cancellationToken);
    }
}
