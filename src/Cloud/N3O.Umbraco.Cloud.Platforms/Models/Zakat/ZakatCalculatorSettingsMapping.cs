using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class ZakatCalculatorSettingsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ZakatCalculatorSettingsContent, ZakatCalculatorSettingsReq>((_, _) => new ZakatCalculatorSettingsReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ZakatCalculatorSettingsContent src, ZakatCalculatorSettingsReq dest, MapperContext ctx) {
        dest.DonationFormState = ctx.Map<OfferingContent, DonationFormStateReq>(src.Offering);
        dest.Sections = src.Sections
                           .Where(x => x.Fields.HasAny())
                           .Select(ctx.Map<ZakatCalculatorSectionSettingsContent, ZakatCalculatorSectionReq>)
                           .ToList();

        dest.DonationFormState.Options = new DonationFormOptionsReq();
        dest.DonationFormState.Options.SuggestedFilters = new DonationFormSuggestedFiltersReq();
        dest.DonationFormState.Options.SuggestedFilters.FundDimensions = new FundDimensionsFilterReq();
        dest.DonationFormState.Options.SuggestedFilters.FundDimensions.Dimension1 = src.FundDimension1?.Name;
        dest.DonationFormState.Options.SuggestedFilters.FundDimensions.Dimension2 = src.FundDimension2?.Name;
        dest.DonationFormState.Options.SuggestedFilters.FundDimensions.Dimension3 = src.FundDimension3?.Name;
        dest.DonationFormState.Options.SuggestedFilters.FundDimensions.Dimension4 = src.FundDimension4?.Name;
    }
}