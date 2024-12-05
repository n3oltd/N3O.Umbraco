using N3O.Umbraco.Elements.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class SponsorshipDonationOptionDataMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SponsorshipDonationOptionContent, SponsorshipDonationOptionData>((_, _) => new SponsorshipDonationOptionData(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(SponsorshipDonationOptionContent src, SponsorshipDonationOptionData dest, MapperContext ctx) {
        dest.SchemeId = src.Scheme.Id;
        dest.SchemeName = src.Scheme.Name;
        dest.AllowedGivingTypeIds = src.Scheme.AllowedGivingTypes.Select(x => x.Id).ToList();
        dest.AllowedDurations = src.Scheme
                                   .AllowedDurations
                                   .Select(ctx.Map<SponsorshipDuration, SponsorshipDurationData>)
                                   .ToList();
        dest.Dimension1Options = src.Scheme
                                    .Dimension1Options
                                    .OrEmpty()
                                    .Select(ctx.Map<FundDimension1Value, FundDimensionValueData>)
                                    .ToList();
        dest.Dimension2Options = src.Scheme
                                    .Dimension2Options
                                    .OrEmpty()
                                    .Select(ctx.Map<FundDimension2Value, FundDimensionValueData>)
                                    .ToList();
        dest.Dimension3Options = src.Scheme
                                    .Dimension3Options
                                    .OrEmpty()
                                    .Select(ctx.Map<FundDimension3Value, FundDimensionValueData>)
                                    .ToList();
        dest.Dimension4Options = src.Scheme
                                    .Dimension4Options
                                    .OrEmpty()
                                    .Select(ctx.Map<FundDimension4Value, FundDimensionValueData>)
                                    .ToList();

        dest.Components = src.Scheme
                             .Components
                             .Select(ctx.Map<SponsorshipComponent, SponsorshipComponentData>)
                             .ToList();
    }
}
