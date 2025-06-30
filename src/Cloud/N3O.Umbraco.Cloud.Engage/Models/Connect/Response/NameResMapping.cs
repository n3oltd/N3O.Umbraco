using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Cloud.Engage.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Engage.Models;

public class NameResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ConnectNameRes, NameRes>((_, _) => new NameRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ConnectNameRes src, NameRes dest, MapperContext ctx) {
        dest.Title = src.Title;
        dest.FirstName = src.FirstName;
        dest.LastName = src.LastName;
    }
}