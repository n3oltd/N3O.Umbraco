using N3O.Umbraco.Elements.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class CategoryEntryDataMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<DonationCategoryContent, CategoryEntryData>((_, _) => new CategoryEntryData(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(DonationCategoryContent src, CategoryEntryData dest, MapperContext ctx) {
        dest.Id = src.Id;
    }
}