using N3O.Umbraco.Blocks;
using N3O.Umbraco.Giving.Cart.Models;

namespace N3O.Umbraco.Giving.Cart.Extensions {
    public static class BlockViewModelExtensions {
        public static CartModel Cart(this IBlockViewModel blockViewModel) {
            return blockViewModel.ModulesData.Get<CartModel>(CartConstants.BlockModuleKeys.Cart);
        }
    }
}
