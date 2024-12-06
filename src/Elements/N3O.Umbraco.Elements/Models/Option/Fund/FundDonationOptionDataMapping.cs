using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using FundDonationOptionContent = N3O.Umbraco.Elements.Content.FundDonationOptionContent;

namespace N3O.Umbraco.Elements.Models;

public class FundDonationOptionDataMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FundDonationOptionContent, FundDonationOptionData>((_, _) => new FundDonationOptionData(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(FundDonationOptionContent src, FundDonationOptionData dest, MapperContext ctx) {
        dest.DonationItemId = src.DonationItem.Id;
        dest.DonationItemName = src.DonationItem.Name;
        dest.AllowedGivingTypeIds = src.DonationItem.AllowedGivingTypes.Select(x => x.Id).ToList();
        dest.Dimension1Options = src.DonationItem
                                    .Dimension1Options
                                    .OrEmpty()
                                    .Select(ctx.Map<FundDimension1Value, FundDimensionValueData>)
                                    .ToList();
        dest.Dimension2Options = src.DonationItem
                                    .Dimension2Options
                                    .OrEmpty()
                                    .Select(ctx.Map<FundDimension2Value, FundDimensionValueData>)
                                    .ToList();
        dest.Dimension3Options = src.DonationItem
                                    .Dimension3Options
                                    .OrEmpty()
                                    .Select(ctx.Map<FundDimension3Value, FundDimensionValueData>)
                                    .ToList();
        dest.Dimension4Options = src.DonationItem
                                    .Dimension4Options
                                    .OrEmpty()
                                    .Select(ctx.Map<FundDimension4Value, FundDimensionValueData>)
                                    .ToList();
        dest.Pricing = ctx.Map<IPricing, PricingData>(src.DonationItem);
        dest.DonationPriceHandles = src.DonationPriceHandles
                                       .OrEmpty()
                                       .Select(ctx.Map<PriceHandleElement, PriceHandleData>)
                                       .ToList();
        dest.RegularGivingPriceHandles = src.RegularGivingPriceHandles
                                            .OrEmpty()
                                            .Select(ctx.Map<PriceHandleElement, PriceHandleData>)
                                            .ToList();
    }
}
