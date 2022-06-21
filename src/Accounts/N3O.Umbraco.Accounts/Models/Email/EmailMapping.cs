using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Accounts.Models;

public class EmailMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<Email, EmailRes>((_, _) => new EmailRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(Email src, EmailRes dest, MapperContext ctx) {
        dest.Address = src.Address;
    }
}
