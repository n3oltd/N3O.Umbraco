using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Data.Models {
    public class UmbracoPropertyInfoMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<UmbracoPropertyInfo, UmbracoPropertyInfoRes>((_, _) => new UmbracoPropertyInfoRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(UmbracoPropertyInfo src, UmbracoPropertyInfoRes dest, MapperContext ctx) {
            dest.Alias = src.Type.Alias;
            dest.Group = src.Group.Name;
            dest.DataType = src.DataType.EditorAlias;
            dest.Name = src.Type.Name;
        }
    }
}