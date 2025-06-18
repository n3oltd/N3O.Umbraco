using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Engage.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crm.Models;

public class ConnectIndividualReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IIndividual, ConnectIndividualReq>((_, _) => new ConnectIndividualReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IIndividual src, ConnectIndividualReq dest, MapperContext ctx) {
        dest.Name = ctx.Map<IName, ConnectNameReq>(src.Name);
    }
}