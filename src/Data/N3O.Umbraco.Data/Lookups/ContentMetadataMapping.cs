using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Data.Lookups {
    public class ContentMetadataMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<ContentMetadata, ContentMetadataRes>((_, _) => new ContentMetadataRes(), Map);
        }

        // Umbraco.Code.MapAll -Id -Name
        private void Map(ContentMetadata src, ContentMetadataRes dest, MapperContext ctx) {
            ctx.Map<INamedLookup, NamedLookupRes>(src, dest);

            dest.AutoSelected = src.AutoSelected;
            dest.DisplayOrder = src.DisplayOrder;
        }
    }
}