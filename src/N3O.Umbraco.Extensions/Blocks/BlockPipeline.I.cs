using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks {
    public interface IBlockPipeline {
        Task<BlockModulesData> RunAsync(IPublishedElement block, CancellationToken cancellationToken = default);
    }
}
