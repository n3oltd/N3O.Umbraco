using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Cloud.Engage.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Engage.Models;

public class ConnectEmailReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IEmail, ConnectEmailReq>((_, _) => new ConnectEmailReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IEmail src, ConnectEmailReq dest, MapperContext ctx) {
        dest.Address = src.Address;
    }
}