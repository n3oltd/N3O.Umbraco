using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedCountryMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<Country, PublishedCountry>((_, _) => new PublishedCountry(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(Country src, PublishedCountry dest, MapperContext ctx) {
        dest.Name = src.Name;
        dest.Id = src.Id;
        dest.Iso2Code = src.Iso2Code;
        dest.Iso3Code = src.Iso3Code;
        dest.DialingCode = src.DialingCode;
        dest.LocalityOptional = src.LocalityOptional;
        dest.PostalCodeOptional = src.PostalCodeOptional;
    }
}