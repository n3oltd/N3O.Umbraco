using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Donations.Content;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Donations.Models {
    public class FundDonationOptionMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<FundDonationOptionContent, FundDonationOptionRes>((_, _) => new FundDonationOptionRes(), Map);
        }

        // Umbraco.Code.MapAll -Id -Name
        private void Map(FundDonationOptionContent src, FundDonationOptionRes dest, MapperContext ctx) {
            dest.DonationItem = src.DonationItem;
            dest.Dimension1 = GetFixedOrDefault(ctx, src.Dimension1, src.DonationItem.Dimension1Options);
            dest.Dimension2 =GetFixedOrDefault(ctx, src.Dimension2, src.DonationItem.Dimension2Options);
            dest.Dimension3 = GetFixedOrDefault(ctx, src.Dimension3, src.DonationItem.Dimension3Options);
            dest.Dimension4 = GetFixedOrDefault(ctx, src.Dimension4, src.DonationItem.Dimension4Options);
            dest.ShowQuantity = src.ShowQuantity;
            dest.HideSingle = src.HideSingle;
            dest.SinglePriceHandles = src.SinglePriceHandles.OrEmpty().Select(ctx.Map<PriceHandleElement, PriceHandleRes>).ToList();
            dest.HideRegular = src.HideRegular;
            dest.RegularPriceHandles = src.RegularPriceHandles.OrEmpty().Select(ctx.Map<PriceHandleElement, PriceHandleRes>).ToList();
        }

        private FixedOrDefaultFundDimensionOptionRes GetFixedOrDefault<TOption>(MapperContext ctx,
                                                                                TOption @fixed,
                                                                                IEnumerable<TOption> options)
            where TOption : FundDimensionOption<TOption> {
            var fixedRes = ctx.Map<FundDimensionOption<TOption>, FundDimensionOptionRes>(@fixed);
            var optionsRes = options.OrEmpty().Select(ctx.Map<FundDimensionOption<TOption>, FundDimensionOptionRes>).ToList();
            
            return FixedOrDefaultFundDimensionOptionRes.For(fixedRes, optionsRes);
        }
    }
}