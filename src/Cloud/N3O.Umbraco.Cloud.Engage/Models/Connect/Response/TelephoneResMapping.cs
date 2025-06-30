using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Cloud.Engage.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Country = N3O.Umbraco.Lookups.Country;

namespace N3O.Umbraco.Cloud.Engage.Models;

public class TelephoneResMapping : IMapDefinition {
    private readonly ILookups _lookups;

    public TelephoneResMapping(ILookups lookups) {
        _lookups = lookups;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ConnectTelephoneRes, TelephoneRes>((_, _) => new TelephoneRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ConnectTelephoneRes src, TelephoneRes dest, MapperContext ctx) {
        dest.Number = src.Number;
        dest.Country = src.Country.IfNotNull(x => _lookups.GetAll<Country>()
                                                          .SingleOrDefault(y => y.Iso2Code.EqualsInvariant(x.ToString())));
    }
}