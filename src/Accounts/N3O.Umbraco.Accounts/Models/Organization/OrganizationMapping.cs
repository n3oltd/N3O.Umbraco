using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Accounts.Models;

public class OrganizationMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<Organization, OrganizationRes>((_, _) => new OrganizationRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(Organization src, OrganizationRes dest, MapperContext ctx) {
        dest.Type = src.Type;
        dest.Name = src.Name;
        dest.Contact = ctx.Map<Name, NameRes>(src.Contact);
    }
}