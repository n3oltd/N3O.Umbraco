using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Accounts.Models {
    public class TelephoneMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<Telephone, TelephoneRes>((_, _) => new TelephoneRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(Telephone src, TelephoneRes dest, MapperContext ctx) {
            dest.Country = src.Country;
            dest.Number = src.Number;
        }
    }
}