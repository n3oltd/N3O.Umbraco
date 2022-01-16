using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages {
    public interface IPagePipeline {
        Task<PageModulesData> RunAsync(IPublishedContent page, CancellationToken cancellationToken = default);
    }
}
