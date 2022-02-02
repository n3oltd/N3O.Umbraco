using N3O.Umbraco.Exceptions;
using N3O.Giving.Models;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Extensions;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models {
    public class DonationOptionMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<DonationOptionContent, DonationOptionRes>((_, _) => new DonationOptionRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(DonationOptionContent src, DonationOptionRes dest, MapperContext ctx) {
            dest.Type = src.Type;
            dest.Dimension1 = GetFixedOrDefault(ctx, src.Dimension1, src.GetFundDimensionOptions().DefaultFundDimension1());
            dest.Dimension2 = GetFixedOrDefault(ctx, src.Dimension2, src.GetFundDimensionOptions().DefaultFundDimension2());
            dest.Dimension3 = GetFixedOrDefault(ctx, src.Dimension3, src.GetFundDimensionOptions().DefaultFundDimension3());
            dest.Dimension4 = GetFixedOrDefault(ctx, src.Dimension4, src.GetFundDimensionOptions().DefaultFundDimension4());
            dest.HideDonation = src.HideDonation;
            dest.HideRegularGiving = src.HideRegularGiving;
            dest.HideQuantity = src.HideQuantity;

            if (src.Type == AllocationTypes.Fund) {
                dest.Fund = src.Fund.IfNotNull(ctx.Map<FundDonationOptionContent, FundDonationOptionRes>);
            } else if (src.Type == AllocationTypes.Sponsorship) {
                dest.Sponsorship = src.Sponsorship.IfNotNull(ctx.Map<SponsorshipDonationOptionContent, SponsorshipDonationOptionRes>);
            } else {
                throw UnrecognisedValueException.For(src.Type);
            }
        }
        
        private FixedOrDefaultFundDimensionValueRes GetFixedOrDefault<TOption>(MapperContext ctx,
                                                                               TOption @fixed,
                                                                               TOption @default)
            where TOption : FundDimensionValue<TOption> {
            var res = new FixedOrDefaultFundDimensionValueRes();
            res.Fixed = @fixed.IfNotNull(ctx.Map<FundDimensionValue<TOption>, FundDimensionValueRes>);

            if (res.Fixed == null) {
                res.Default = @default.IfNotNull(ctx.Map<FundDimensionValue<TOption>, FundDimensionValueRes>);
            }
            
            return res;
        }
    }
}