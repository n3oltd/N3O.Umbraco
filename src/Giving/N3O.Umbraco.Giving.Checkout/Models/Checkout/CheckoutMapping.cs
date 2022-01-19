using N3O.Umbraco.Accounts.Models;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class CheckoutMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<Entities.Checkout, CheckoutRes>((_, _) => new CheckoutRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(Entities.Checkout src, CheckoutRes dest, MapperContext ctx) {
            dest.Account = ctx.Map<Account, AccountRes>(src.Account);
        }
    }
}