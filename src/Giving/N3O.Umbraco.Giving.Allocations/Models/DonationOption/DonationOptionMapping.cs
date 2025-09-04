using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class DonationOptionMapping : IMapDefinition {
    private readonly ILookups _lookups;
    
    public DonationOptionMapping(ILookups lookups) {
        _lookups = lookups;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<DonationOptionContent, DonationOptionRes>((_, _) => new DonationOptionRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(DonationOptionContent src, DonationOptionRes dest, MapperContext ctx) {
        dest.Id = src.Id;
        dest.Name = src.Name;
        dest.CampaignName = src.CampaignName;
        dest.Type = src.Type;
        dest.DefaultGivingType = src.DefaultGivingType;
        dest.Dimension1 = GetInitial(ctx, src.GetDimension1(_lookups), src.GetFundDimensionOptions(_lookups).DefaultFundDimension1(), src.GetFundDimensionOptions(_lookups).Dimension1);
        dest.Dimension2 = GetInitial(ctx, src.GetDimension2(_lookups), src.GetFundDimensionOptions(_lookups).DefaultFundDimension2(), src.GetFundDimensionOptions(_lookups).Dimension2);
        dest.Dimension3 = GetInitial(ctx, src.GetDimension3(_lookups), src.GetFundDimensionOptions(_lookups).DefaultFundDimension3(), src.GetFundDimensionOptions(_lookups).Dimension3);
        dest.Dimension4 = GetInitial(ctx, src.GetDimension4(_lookups), src.GetFundDimensionOptions(_lookups).DefaultFundDimension4(), src.GetFundDimensionOptions(_lookups).Dimension4);
        dest.HideDonation = src.HideDonation;
        dest.HideRegularGiving = src.HideRegularGiving;
        dest.HideQuantity = src.HideQuantity;

        if (src.Type == AllocationTypes.Fund) {
            dest.Fund = src.Fund.IfNotNull(ctx.Map<FundDonationOptionContent, FundDonationOptionRes>);
        } else if (src.Type == AllocationTypes.Sponsorship) {
            dest.Sponsorship = src.Sponsorship.IfNotNull(ctx.Map<SponsorshipDonationOptionContent, SponsorshipDonationOptionRes>);
        } else if (src.Type == AllocationTypes.Feedback) {
            dest.Feedback = src.Feedback.IfNotNull(ctx.Map<FeedbackDonationOptionContent, FeedbackDonationOptionRes>);
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
