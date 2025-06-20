using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Cloud.Engage.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Engage.Models;

public class ConnectOrganizationReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IOrganization, ConnectOrganizationReq>((_, _) => new ConnectOrganizationReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IOrganization src, ConnectOrganizationReq dest, MapperContext ctx) {
        dest.Name = src.Name;
        dest.Type = src.Type.Name;
        dest.Contact = ctx.Map<IName, ConnectNameReq>(src.Contact);
    }
}