using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Lookups;

public class NestedPropertyType : PropertyType<NestedValueReq> {
    public NestedPropertyType()
        : base("nested",
               (ctx, src, dest) => dest.Nested = ctx.Map<PublishedContentProperty, NestedValueRes>(src),
               UmbracoPropertyEditors.Aliases.NestedContent) { }

    protected override Task UpdatePropertyAsync(IContentBuilder contentBuilder,
                                                string alias,
                                                NestedValueReq data) {
        var nestedBuilder = contentBuilder.Nested(alias);

        foreach (var nestedValue in data.Items) {
            var builder = nestedBuilder.Add(nestedValue.ContentTypeAlias);

            foreach (var property in nestedValue.Properties) {
                property.Type.UpdatePropertyAsync(builder, property.Alias, property.Value.Value);
            }
        }
        
        return Task.CompletedTask;
    }
}