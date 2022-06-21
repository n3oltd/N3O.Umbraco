using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Checkout.Models;

public class RegularGivingOptionsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<RegularGivingOptions, RegularGivingOptionsRes>((_, _) => new RegularGivingOptionsRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(RegularGivingOptions src, RegularGivingOptionsRes dest, MapperContext ctx) {
        dest.PreferredCollectionDay = src.PreferredCollectionDay;
        dest.Frequency = src.Frequency;
        dest.FirstCollectionDate = src.FirstCollectionDate;
    }
}
