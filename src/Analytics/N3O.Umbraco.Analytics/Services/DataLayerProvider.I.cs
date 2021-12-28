using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Analytics {
    public interface IDataLayerProvider {
        bool IsProviderFor(IPublishedContent page);
        Task<IEnumerable<object>> GetAsync(IPublishedContent page, CancellationToken cancellationToken = default);
    }
}
