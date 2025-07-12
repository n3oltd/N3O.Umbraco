using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Cloud.Engage.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;
using Country = N3O.Umbraco.Lookups.Country;

namespace N3O.Umbraco.Cloud.Engage.Models;

public class AddressResMapping : IMapDefinition {
    private readonly ILookups _lookups;

    public AddressResMapping(ILookups lookups) {
        _lookups = lookups;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ConnectAddressRes, AddressRes>((_, _) => new AddressRes(), Map);
    }

    // Umbraco.Code.MapAll -Line4
    private void Map(ConnectAddressRes src, AddressRes dest, MapperContext ctx) {
        dest.Line1 = src.Line1;
        dest.Line2 = src.Line2;
        dest.Line3 = src.Line3;
        dest.Locality = src.Locality;
        dest.AdministrativeArea = src.AdministrativeArea;
        dest.PostalCode = src.PostalCode;
        dest.Country = _lookups.GetAll<Country>().FindByCode(src.Country?.ToString());
    }
}