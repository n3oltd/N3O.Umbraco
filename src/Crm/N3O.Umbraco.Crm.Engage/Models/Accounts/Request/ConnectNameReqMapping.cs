using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Engage.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crm.Models;

public class ConnectNameReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IName, ConnectNameReq>((_, _) => new ConnectNameReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IName src, ConnectNameReq dest, MapperContext ctx) {
        dest.Title = src.Title;
        dest.FirstName = src.FirstName;
        dest.LastName = src.LastName;
    }
}