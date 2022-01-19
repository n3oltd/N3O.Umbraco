using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Accounts.Models {
    public class NameMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<Name, NameRes>((_, _) => new NameRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(Name src, NameRes dest, MapperContext ctx) {
            dest.Title = src.Title;
            dest.FirstName = src.FirstName;
            dest.LastName = src.LastName;
        }
    }
}