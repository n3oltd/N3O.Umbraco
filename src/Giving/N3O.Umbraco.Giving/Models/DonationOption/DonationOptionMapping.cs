using N3O.Umbraco.Exceptions;
using N3O.Giving.Models;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class DonationOptionMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<DonationOptionContent, DonationOptionRes>((_, _) => new DonationOptionRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(DonationOptionContent src, DonationOptionRes dest, MapperContext ctx) {
        dest.Name = src.Name;
        dest.Type = src.Type;
        dest.DefaultGivingType = src.DefaultGivingType;
        dest.Dimension1 = GetInitial(ctx, src.Dimension1, src.GetFundDimensionOptions().DefaultFundDimension1(), src.GetFundDimensionOptions().Dimension1Options);
        dest.Dimension2 = GetInitial(ctx, src.Dimension2, src.GetFundDimensionOptions().DefaultFundDimension2(), src.GetFundDimensionOptions().Dimension2Options);
        dest.Dimension3 = GetInitial(ctx, src.Dimension3, src.GetFundDimensionOptions().DefaultFundDimension3(), src.GetFundDimensionOptions().Dimension3Options);
        dest.Dimension4 = GetInitial(ctx, src.Dimension4, src.GetFundDimensionOptions().DefaultFundDimension4(), src.GetFundDimensionOptions().Dimension4Options);
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

    private InitialFundDimensionValueRes GetInitial<TValue>(MapperContext ctx,
                                                            TValue @fixed,
                                                            TValue @default,
                                                            IEnumerable<TValue> fundDimensionValues)
        where TValue : FundDimensionValue<TValue> {
        var res = new InitialFundDimensionValueRes();
        res.Fixed = @fixed.IfNotNull(ctx.Map<TValue, FundDimensionValueRes>);

        if (res.Fixed == null) {
            res.Default = @default.IfNotNull(ctx.Map<TValue, FundDimensionValueRes>);
        }

        if (res.Fixed == null && res.Default == null) {
            var values = fundDimensionValues.OrEmpty().ToList();

            res.Suggested = ctx.Map<TValue, FundDimensionValueRes>(values.FirstOrDefault());
        }

        return res;
    }
}
