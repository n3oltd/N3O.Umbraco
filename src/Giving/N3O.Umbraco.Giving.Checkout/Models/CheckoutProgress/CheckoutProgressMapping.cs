using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class CheckoutProgressMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<CheckoutProgress, CheckoutProgressRes>((_, _) => new CheckoutProgressRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(CheckoutProgress src, CheckoutProgressRes dest, MapperContext ctx) {
            dest.Current = src.Current;
            dest.Remaining = src.Remaining;
            dest.Required = src.Required;
        }
    }
}