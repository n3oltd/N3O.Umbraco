using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Cloud.Engage.Clients;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Engage.Models;

public class OrganizationResMapping : IMapDefinition {
    private readonly ILookups _lookups;

    public OrganizationResMapping(ILookups lookups) {
        _lookups = lookups;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ConnectOrganizationRes, OrganizationRes>((_, _) => new OrganizationRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ConnectOrganizationRes src, OrganizationRes dest, MapperContext ctx) {
        dest.Name = src.Name;
        dest.Type = _lookups.FindByIdOrName<OrganizationType>(src.Type);
        dest.Contact = ctx.Map<ConnectNameRes, NameRes>(src.Contact);
    }
}