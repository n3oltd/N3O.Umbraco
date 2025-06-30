using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Cloud.Engage.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Engage.Models;

public class EmailResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ConnectEmailRes, EmailRes>((_, _) => new EmailRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ConnectEmailRes src, EmailRes dest, MapperContext ctx) {
        dest.Address = src.Address;
    }
}