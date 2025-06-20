using N3O.Umbraco.Cloud.Platforms.Content;
using MuslimHands.Website.Connect.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class FundDimensionValuesPubMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IFundDimensionsValues, PublishedFundDimensionValues>((_, _) => new PublishedFundDimensionValues(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IFundDimensionsValues src, PublishedFundDimensionValues dest, MapperContext ctx) {
        dest.Dimension1 = src.Dimension1?.Name;
        dest.Dimension2 = src.Dimension2?.Name;
        dest.Dimension3 = src.Dimension3?.Name;
        dest.Dimension4 = null;
    }
}
