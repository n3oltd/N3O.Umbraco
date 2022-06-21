using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Models;

public class PaymentMethodMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PaymentMethod, PaymentMethodRes>((_, _) => new PaymentMethodRes(), Map);
    }

    // Umbraco.Code.MapAll -Id -Name
    private void Map(PaymentMethod src, PaymentMethodRes dest, MapperContext ctx) {
        ctx.Map<INamedLookup, NamedLookupRes>(src, dest);
    }
}
