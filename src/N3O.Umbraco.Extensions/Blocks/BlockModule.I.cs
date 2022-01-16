using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks {
    public interface IBlockModule {
        string Key { get; }

        bool ShouldExecute(IPublishedElement block); 
        Task<object> ExecuteAsync(IPublishedElement block, CancellationToken cancellationToken);
    }
}
