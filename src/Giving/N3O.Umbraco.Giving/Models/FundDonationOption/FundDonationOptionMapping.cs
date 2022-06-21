using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Content;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class FundDonationOptionMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FundDonationOptionContent, FundDonationOptionRes>((_, _) => new FundDonationOptionRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(FundDonationOptionContent src, FundDonationOptionRes dest, MapperContext ctx) {
        dest.DonationItem = src.DonationItem;
        dest.DonationPriceHandles = src.DonationPriceHandles
                                       .OrEmpty()
                                       .Select(ctx.Map<PriceHandleElement, PriceHandleRes>)
                                       .ToList();
        dest.RegularGivingPriceHandles = src.RegularGivingPriceHandles
                                            .OrEmpty()
                                            .Select(ctx.Map<PriceHandleElement, PriceHandleRes>)
                                            .ToList();
    }
}
