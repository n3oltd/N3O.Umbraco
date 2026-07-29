using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Canonical;

public interface ICanonicalUrlProvider {
    Task<string> GetUrlAsync(IPublishedContent content);
    Task<bool> IsProviderForAsync(IPublishedContent content);
}
