using N3O.Umbraco.Elements.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class GeneralCategoryDataMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<GeneralDonationCategoryContent, GeneralCategoryData>((_, _) => new GeneralCategoryData(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(GeneralDonationCategoryContent src, GeneralCategoryData dest, MapperContext ctx) { }
}