using N3O.Umbraco.Crowdfunding.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class TagResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<TagContent, TagRes>((_, _) => new TagRes(), Map);
    }
    
    private void Map(TagContent src, TagRes dest, MapperContext ctx) {
        dest.Name = src.Name;
        dest.IconUrl = src.Category.Icon.Src;
    }
}