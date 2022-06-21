using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Models;

public class AccountsLookupsRes : LookupsRes {
    [FromLookupType(AccountsLookupTypes.ConsentCategories)]
    public IEnumerable<NamedLookupRes> ConsentCategories { get; set; }

    [FromLookupType(AccountsLookupTypes.ConsentChannels)]
    public IEnumerable<NamedLookupRes> ConsentChannels { get; set; }
    
    [FromLookupType(AccountsLookupTypes.ConsentResponses)]
    public IEnumerable<NamedLookupRes> ConsentResponses { get; set; }

    [FromLookupType(AccountsLookupTypes.Countries)]
    public IEnumerable<CountryRes> Countries { get; set; }
    
    [FromLookupType(AccountsLookupTypes.TaxStatuses)]
    public IEnumerable<NamedLookupRes> TaxStatuses { get; set; }
}
