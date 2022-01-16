using N3O.Umbraco.Blocks;
using N3O.Umbraco.Search.Models;

namespace N3O.Umbraco.Search.Extensions {
    public static class BlockViewModelExtensions {
        public static SearchResults Search(this IBlockViewModel blockViewModel) {
            return blockViewModel.ModulesData.Get<SearchResults>(SearchConstants.BlockModuleKeys.Search);
        }
    }
}
