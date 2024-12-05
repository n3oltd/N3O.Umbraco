using N3O.Umbraco.Elements.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class DonationOptionPartialMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<DonationOptionContent, DonationOptionPartial>((_, _) => new DonationOptionPartial(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(DonationOptionContent src, DonationOptionPartial dest, MapperContext ctx) {
        dest.Id = src.Id;
        dest.Name = src.Name;
        dest.TypeId = src.Type.Id;
        dest.DefaultGivingTypeId = src.DefaultGivingType.Id;
        dest.Dimension1 = GetInitial(ctx, src.Dimension1, src.GetFundDimensionOptions().DefaultFundDimension1(), src.GetFundDimensionOptions().Dimension1Options);
        dest.Dimension2 = GetInitial(ctx, src.Dimension2, src.GetFundDimensionOptions().DefaultFundDimension2(), src.GetFundDimensionOptions().Dimension2Options);
        dest.Dimension3 = GetInitial(ctx, src.Dimension3, src.GetFundDimensionOptions().DefaultFundDimension3(), src.GetFundDimensionOptions().Dimension3Options);
        dest.Dimension4 = GetInitial(ctx, src.Dimension4, src.GetFundDimensionOptions().DefaultFundDimension4(), src.GetFundDimensionOptions().Dimension4Options);
        dest.HideDonation = src.HideDonation;
        dest.HideRegularGiving = src.HideRegularGiving;
        dest.HideQuantity = src.HideQuantity;
        dest.Synopsis = src.Synopsis;
        dest.Description = src.Description;
        dest.Feedback = src.Feedback.IfNotNull(ctx.Map<FeedbackDonationOptionContent, FeedbackDonationOptionData>);
        dest.Fund = src.Fund.IfNotNull(ctx.Map<FundDonationOptionContent, FundDonationOptionData>);
        dest.Sponsorship = src.Sponsorship.IfNotNull(ctx.Map<SponsorshipDonationOptionContent, SponsorshipDonationOptionData>);
    }

    private InitialFundDimensionValueData GetInitial<TValue>(MapperContext ctx,
                                                            TValue @fixed,
                                                            TValue @default,
                                                            IEnumerable<TValue> fundDimensionValues)
        where TValue : FundDimensionValue<TValue> {
        var data = new InitialFundDimensionValueData();
        data.Fixed = @fixed.IfNotNull(ctx.Map<TValue, FundDimensionValueData>);

        if (data.Fixed == null) {
            data.Default = @default.IfNotNull(ctx.Map<TValue, FundDimensionValueData>);
        }

        if (data.Fixed == null && data.Default == null) {
            var values = fundDimensionValues.OrEmpty().ToList();

            data.Suggested = ctx.Map<TValue, FundDimensionValueData>(values.FirstOrDefault());
        }

        return data;
    }
}