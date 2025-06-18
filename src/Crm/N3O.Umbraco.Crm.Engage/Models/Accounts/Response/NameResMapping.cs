using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Engage.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crm.Models;

public class NameResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ConnectNameRes, NameRes>((_, _) => new NameRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ConnectNameRes src, NameRes dest, MapperContext ctx) {
        dest.Title = src.Title?.Name;
        dest.FirstName = src.FirstName;
        dest.LastName = src.LastName;
    }
}