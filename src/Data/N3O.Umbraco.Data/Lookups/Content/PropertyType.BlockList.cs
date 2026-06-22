using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Lookups;

public class BlockListPropertyType : PropertyType<BlockListValueReq> {
    public BlockListPropertyType()
        : base("blockList",
               (ctx, src, dest) => dest.BlockList = ctx.Map<PublishedContentProperty, BlockListValueRes>(src),
               (ctx, src) => ctx.Map<ContentPropertyConfiguration, BlockListConfigurationRes>(src),
               UmbracoPropertyEditors.Aliases.BlockList) { }

    protected override async Task UpdatePropertyAsync(IContentBuilder contentBuilder,
                                                string alias,
                                                BlockListValueReq data) {
        var blockListBuilder = contentBuilder.BlockList(alias);

        foreach (var blockListValue in data.Items) {
            var builder = blockListBuilder.Add(blockListValue.ContentTypeAlias);

            foreach (var property in blockListValue.Properties) {
                await property.Type.UpdatePropertyAsync(builder, property.Alias, property.Value.Value);
            }
        }
    }
}
