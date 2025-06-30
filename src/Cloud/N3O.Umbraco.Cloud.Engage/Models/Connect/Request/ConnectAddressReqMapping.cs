using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Cloud.Engage.Clients;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;
using ConnectCountry = N3O.Umbraco.Cloud.Engage.Clients.Country;

namespace N3O.Umbraco.Cloud.Engage.Models;

public class ConnectAddressReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IAddress, ConnectAddressReq>((_, _) => new ConnectAddressReq(), Map);
    }

    // Umbraco.Code.MapAll -Line4
    private void Map(IAddress src, ConnectAddressReq dest, MapperContext ctx) {
        dest.Line1 = src.Line1;
        dest.Line2 = src.Line2;
        dest.Line3 = src.Line3;
        dest.Locality = src.Locality;
        dest.AdministrativeArea = src.AdministrativeArea;
        dest.PostalCode = src.PostalCode;
        dest.Country = src.Country?.Iso2Code.ToEnum<ConnectCountry>();
    }
}