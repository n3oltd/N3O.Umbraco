using System.Collections.Generic;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Blocks;

public interface IBlocksBuilder {
    IEnumerable<BlockDefinition> Build(IUmbracoBuilder builder);
}