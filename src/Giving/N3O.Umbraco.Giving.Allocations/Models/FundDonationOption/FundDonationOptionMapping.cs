using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Lookups;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FundDonationOptionMapping : IMapDefinition {
    private readonly ILookups _lookups;
    
    public FundDonationOptionMapping(ILookups lookups) {
        _lookups = lookups;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FundDonationOptionContent, FundDonationOptionRes>((_, _) => new FundDonationOptionRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(FundDonationOptionContent src, FundDonationOptionRes dest, MapperContext ctx) {
        dest.DonationItem = src.GetDonationItem(_lookups);
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
