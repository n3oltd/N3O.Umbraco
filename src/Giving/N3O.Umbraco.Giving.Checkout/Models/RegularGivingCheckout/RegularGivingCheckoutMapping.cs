using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Payments.Models;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Checkout.Models;

public class RegularGivingCheckoutMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<RegularGivingCheckout, RegularGivingCheckoutRes>((_, _) => new RegularGivingCheckoutRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(RegularGivingCheckout src, RegularGivingCheckoutRes dest, MapperContext ctx) {
        dest.Allocations = src.Allocations
                              .OrEmpty()
                              .Select(ctx.Map<Allocation, AllocationRes>)
                              .ToList();
        dest.Credential = ctx.Map<Credential, CredentialRes>(src.Credential);
        dest.Options = ctx.Map<RegularGivingOptions, RegularGivingOptionsRes>(src.Options);
        dest.Total = ctx.Map<Money, MoneyRes>(src.Total);
        dest.IsComplete = src.IsComplete;
        dest.IsRequired = src.IsRequired;
    }
}
