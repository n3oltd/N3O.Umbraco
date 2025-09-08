using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks;

public interface IBlocksRendererPostProcessor {
    Task<string> ProcessAsync(IPublishedContent content, string html);
}