using N3O.Umbraco.Elements.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class OptionEntryDataMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<DonationOptionContent, OptionEntryData>((_, _) => new OptionEntryData(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(DonationOptionContent src, OptionEntryData dest, MapperContext ctx) {
        dest.Id = src.Id;
    }
}