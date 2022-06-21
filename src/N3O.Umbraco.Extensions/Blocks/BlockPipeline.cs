using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks;

public class BlockPipeline : IBlockPipeline {
    private readonly IReadOnlyList<IBlockModule> _modules;

    public BlockPipeline(IEnumerable<IBlockModule> modules) {
        _modules = modules.ToList();
    }

    public async Task<BlockModulesData> RunAsync(IPublishedElement block,
                                                 CancellationToken cancellationToken = default) {
        var modulesData = new BlockModulesData();
    
        foreach (var module in _modules.Where(x => x.ShouldExecute(block))) {
            var data = await module.ExecuteAsync(block, cancellationToken);

            if (data != null) {
                modulesData.Add(module.Key, data);
            }
        }

        return modulesData;
    }
}
