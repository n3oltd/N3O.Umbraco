using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Lookups {
    public class CountryMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<Country, CountryRes>((_, _) => new CountryRes(), Map);
        }

        // Umbraco.Code.MapAll -Id -Name
        private void Map(Country src, CountryRes dest, MapperContext ctx) {
            ctx.Map<INamedLookup, NamedLookupRes>(src, dest);
            
            dest.Iso2Code = src.Iso2Code;
            dest.Iso3Code = src.Iso3Code;
            dest.LocalityOptional = src.LocalityOptional;
            dest.PostalCodeOptional = src.PostalCodeOptional;
        }
    }
}