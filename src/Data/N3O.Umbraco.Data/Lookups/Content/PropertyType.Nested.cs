using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Lookups;

public class NestedPropertyType : PropertyType<NestedValueReq> {
    // TODO Migration Review: Nested Content was removed in Umbraco 14; existing data is migrated to Block List by
    // NestedContentToBlockListMigration, so this lookup now targets the Block List editor.
    // The lookup id ("nested") and value/config models are retained for schema back-compat.
    public NestedPropertyType()
        : base("nested",
               (ctx, src, dest) => dest.Nested = ctx.Map<PublishedContentProperty, NestedValueRes>(src),
               (ctx, src) => ctx.Map<ContentPropertyConfiguration, NestedConfigurationRes>(src),
               UmbracoPropertyEditors.Aliases.BlockList) { }

    protected override async Task UpdatePropertyAsync(IContentBuilder contentBuilder,
                                                string alias,
                                                NestedValueReq data) {
        var nestedBuilder = contentBuilder.BlockList(alias);

        foreach (var nestedValue in data.Items) {
            var builder = nestedBuilder.Add(nestedValue.ContentTypeAlias);

            foreach (var property in nestedValue.Properties) {
                await property.Type.UpdatePropertyAsync(builder, property.Alias, property.Value.Value);
            }
        }
    }
}