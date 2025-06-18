using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Engage.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crm.Models;

public class IndividualResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ConnectIndividualRes, IndividualRes>((_, _) => new IndividualRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ConnectIndividualRes src, IndividualRes dest, MapperContext ctx) {
        dest.Name = ctx.Map<ConnectNameRes, NameRes>(src.Name);
    }
}