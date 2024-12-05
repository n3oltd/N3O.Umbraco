using N3O.Umbraco.Elements.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class DimensionCategoryDataMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<DimensionDonationCategoryContent, DimensionCategoryData>((_, _) => new DimensionCategoryData(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(DimensionDonationCategoryContent src, DimensionCategoryData dest, MapperContext ctx) {
        dest.DimensionNumber = src.DimensionNumber;
    }
}