using N3O.Umbraco.Crm.Engage.Clients;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.TaxRelief.Lookups;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crm.Models;

public class TaxStatusResMapping : IMapDefinition {
    private readonly ILookups _lookups;

    public TaxStatusResMapping(ILookups lookups) {
        _lookups = lookups;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ConnectTaxStatusRes, TaxStatus>(Map);
    }

    private TaxStatus Map(ConnectTaxStatusRes src, MapperContext ctx) {
        return _lookups.GetAll<TaxStatus>().SingleOrDefault(x => x.ToBool() == src.CanClaim);
    }
}