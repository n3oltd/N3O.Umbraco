using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Lookups {
    public class NamedLookupMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<INamedLookup, NamedLookupRes>((_, _) => new NamedLookupRes(), Map);
        }

        // Umbraco.Code.MapAll -Id
        private void Map(INamedLookup src, NamedLookupRes dest, MapperContext ctx) {
            ctx.Map<ILookup, LookupRes>(src, dest);
            
            dest.Name = src.Name;
        }
    }
}