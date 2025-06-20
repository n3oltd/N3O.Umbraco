using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Cloud.Engage.Clients;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Mapping;
using ConnectCountry = N3O.Umbraco.Cloud.Engage.Clients.Country;

namespace N3O.Umbraco.Cloud.Engage.Models;

public class ConnectTelephoneReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ITelephone, ConnectTelephoneReq>((_, _) => new ConnectTelephoneReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ITelephone src, ConnectTelephoneReq dest, MapperContext ctx) {
        dest.Number = src.Number;
        dest.Country = src.Country.IfNotNull(x => x.Iso2Code, Enum.Parse<ConnectCountry>);
    }
}