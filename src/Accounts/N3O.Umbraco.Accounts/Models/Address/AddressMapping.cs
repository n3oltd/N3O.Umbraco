using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Accounts.Models;

public class AddressMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<Address, AddressRes>((_, _) => new AddressRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(Address src, AddressRes dest, MapperContext ctx) {
        dest.Line1 = src.Line1;
        dest.Line2 = src.Line2;
        dest.Line3 = src.Line3;
        dest.Locality = src.Locality;
        dest.AdministrativeArea = src.AdministrativeArea;
        dest.PostalCode = src.PostalCode;
        dest.Country = src.Country;
    }
}
