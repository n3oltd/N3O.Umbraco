using N3O.Umbraco.Elements.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class EphemeralCategoryDataMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<EphemeralDonationCategoryContent, EphemeralCategoryData>((_, _) => new EphemeralCategoryData(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(EphemeralDonationCategoryContent src, EphemeralCategoryData dest, MapperContext ctx) {
        dest.StartOn = src.StartOn;
        dest.EndOn = src.EndOn;
    }
}