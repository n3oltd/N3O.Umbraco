using N3O.Umbraco.Extensions;
using N3O.Umbraco.FundDimensions;
using N3O.Umbraco.Giving.Sponsorships.Lookups;
using N3O.Umbraco.Lookups;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Sponsorships.Models {
    public class SponsorshipSchemeMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<SponsorshipScheme, SponsorshipSchemeRes>((_, _) => new SponsorshipSchemeRes(), Map);
        }

        // Umbraco.Code.MapAll -Id -Name
        private void Map(SponsorshipScheme src, SponsorshipSchemeRes dest, MapperContext ctx) {
            ctx.Map<INamedLookup, NamedLookupRes>(src, dest);

            dest.AllowedGivingTypes = src.AllowedGivingTypes;
            dest.Dimension1Options = src.Dimension1Options
                                        .OrEmpty()
                                        .Select(ctx.Map<FundDimensionOption<FundDimension1Option>, FundDimensionOptionRes>)
                                        .ToList();
            dest.Dimension2Options = src.Dimension2Options
                                        .OrEmpty()
                                        .Select(ctx.Map<FundDimensionOption<FundDimension2Option>, FundDimensionOptionRes>)
                                        .ToList();
            dest.Dimension3Options = src.Dimension3Options
                                        .OrEmpty()
                                        .Select(ctx.Map<FundDimensionOption<FundDimension3Option>, FundDimensionOptionRes>)
                                        .ToList();
            dest.Dimension4Options = src.Dimension4Options
                                        .OrEmpty()
                                        .Select(ctx.Map<FundDimensionOption<FundDimension4Option>, FundDimensionOptionRes>)
                                        .ToList();
            dest.Components = src.Components.OrEmpty().Select(ctx.Map<SponsorshipSchemeComponent, SponsorshipSchemeComponentRes>);
        }
    }
}