using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Models;

public class NestedValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPublishedProperty, NestedValueRes>((_, _) => new NestedValueRes(), Map);
    }

    private void Map(IPublishedProperty src, NestedValueRes dest, MapperContext ctx) {
        var elements = src.GetValue() as List<IPublishedElement>;
        var items = new List<NestedItemRes>();
        
        foreach (var element in elements.OrEmpty()) {
            items.Add(PopulateNestedItem(ctx, element));
        }
        
        dest.Items = items;
        dest.Schema = ctx.Map<IPublishedProperty, NestedSchemaRes>(src);
    }

    private NestedItemRes PopulateNestedItem(MapperContext ctx, IPublishedElement element) {
        var properties = element.Properties.Select(ctx.Map<IPublishedProperty, ContentPropertyRes>);
        
        var res = new NestedItemRes();
        res.ContentTypeAlias = element.ContentType.Alias;
        res.Properties = properties;

        return res;
    }
}