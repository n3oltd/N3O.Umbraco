using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Models;

public class NestedValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PublishedContentProperty, NestedValueRes>((_, _) => new NestedValueRes(), Map);
    }

    private void Map(PublishedContentProperty src, NestedValueRes dest, MapperContext ctx) {
        var elements = src.Property.GetValue() as List<IPublishedElement>;
        var items = new List<NestedItemRes>();
        
        foreach (var element in elements.OrEmpty()) {
            items.Add(PopulateNestedItem(ctx, element));
        }
        
        dest.Items = items;
        dest.Schema = ctx.Map<PublishedContentProperty, NestedSchemaRes>(src);
        dest.Configuration = ctx.Map<PublishedContentProperty, NestedConfigurationRes>(src);
    }

    private NestedItemRes PopulateNestedItem(MapperContext ctx, IPublishedElement element) {
        var publishedContentProperties = element.Properties.Select(x => new PublishedContentProperty(element.ContentType.Alias, x));
        
        var properties = publishedContentProperties.Select(ctx.Map<PublishedContentProperty, ContentPropertyValueRes>);
        
        var res = new NestedItemRes();
        res.ContentTypeAlias = element.ContentType.Alias;
        res.Properties = properties;

        return res;
    }
}