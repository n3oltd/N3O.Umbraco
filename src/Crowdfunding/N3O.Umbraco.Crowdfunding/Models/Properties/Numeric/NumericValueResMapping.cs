using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Models;

public class NumericValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPublishedProperty, NumericValueRes>((_, _) => new NumericValueRes(), Map);
    }

    private void Map(IPublishedProperty src, NumericValueRes dest, MapperContext ctx) {
        dest.Value = (decimal?) src.GetValue();
    }
}