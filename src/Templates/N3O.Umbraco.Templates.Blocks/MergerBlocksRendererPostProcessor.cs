using N3O.Umbraco.Blocks;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Templates.Blocks;

public class MergerBlocksRendererPostProcessor : IBlocksRendererPostProcessor {
    private readonly IMerger _merger;

    public MergerBlocksRendererPostProcessor(IMerger merger) {
        _merger = merger;
    }
    
    public async Task<string> ProcessAsync(IPublishedContent content, string html) {
        return await _merger.MergeForAsync(content, html);
    }
}