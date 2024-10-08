using N3O.Umbraco.Data.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Data.Models;

public class TextareaValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PublishedContentProperty, TextareaValueRes>((_, _) => new TextareaValueRes(), Map);
    }

    private void Map(PublishedContentProperty src, TextareaValueRes dest, MapperContext ctx) {
        dest.Value = src.Property.GetValue() as string;
        dest.Configuration = (TextareaConfigurationRes) PropertyTypes.Textarea.GetConfigurationRes(ctx,
                                                                                                  src.ContentTypeAlias,
                                                                                                  src.Property.Alias);
    }
}