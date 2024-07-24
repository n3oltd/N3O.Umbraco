using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Models;

public class TextareaValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPublishedProperty, TextareaValueRes>((_, _) => new TextareaValueRes(), Map);
    }

    private void Map(IPublishedProperty src, TextareaValueRes dest, MapperContext ctx) {
        dest.Value = src.GetValue() as string;
    }
}