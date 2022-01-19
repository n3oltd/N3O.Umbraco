using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Donations.Content;
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
            dest.Dimension1 = GetFixedOrDefault(ctx, src.Dimension1, src.DonationItem.DefaultFundDimension1());
            dest.Dimension2 =GetFixedOrDefault(ctx, src.Dimension2, src.DonationItem.DefaultFundDimension2());
            dest.Dimension3 = GetFixedOrDefault(ctx, src.Dimension3, src.DonationItem.DefaultFundDimension3());
            dest.Dimension4 = GetFixedOrDefault(ctx, src.Dimension4, src.DonationItem.DefaultFundDimension4());
            dest.ShowQuantity = src.ShowQuantity;
            dest.HideSingle = src.HideSingle;
            dest.SinglePriceHandles = src.SinglePriceHandles.OrEmpty().Select(ctx.Map<PriceHandleElement, PriceHandleRes>).ToList();
            dest.HideRegular = src.HideRegular;
            dest.RegularPriceHandles = src.RegularPriceHandles.OrEmpty().Select(ctx.Map<PriceHandleElement, PriceHandleRes>).ToList();
        }

        private FixedOrDefaultFundDimensionOptionRes GetFixedOrDefault<TOption>(MapperContext ctx,
                                                                                TOption @fixed,
                                                                                TOption @default)
            where TOption : FundDimensionOption<TOption> {
            var res = new FixedOrDefaultFundDimensionOptionRes();
            res.Fixed = @fixed.IfNotNull(ctx.Map<FundDimensionOption<TOption>, FundDimensionOptionRes>);

            if (res.Fixed == null) {
                res.Default = @default.IfNotNull(ctx.Map<FundDimensionOption<TOption>, FundDimensionOptionRes>);
            }
            
            return res;
        }
    }
}