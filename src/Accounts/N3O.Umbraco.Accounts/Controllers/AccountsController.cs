using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Accounts.Queries;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.TaxRelief.Lookups;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Accounts.Controllers;

[ApiDocument(AccountsConstants.ApiName)]
public class AccountsController : LookupsController<AccountsLookupsRes> {
    private readonly IMediator _mediator;

    public AccountsController(ILookups lookups, IUmbracoMapper mapper, IMediator mediator)
        : base(lookups, mapper) {
        _mediator = mediator;
    }
    
    [HttpGet("dataEntrySettings")]
    public async Task<ActionResult<DataEntrySettings>> GetDataEntrySettings() {
        var res = await _mediator.SendAsync<GetDataEntrySettingsQuery, None, DataEntrySettings>(None.Empty);

        return Ok(res);
    }

    [HttpGet("lookups/" + AccountsLookupTypes.ConsentCategories)]
    public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupConsentCategories() {
        var res = await GetLookupsAsync<ConsentCategory>();

        return Ok(res);
    }
    
    [HttpGet("lookups/" + AccountsLookupTypes.ConsentChannels)]
    public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupConsentChannels() {
        var res = await GetLookupsAsync<ConsentChannel>();

        return Ok(res);
    }
    
    [HttpGet("lookups/" + AccountsLookupTypes.ConsentResponses)]
    public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupConsentResponses() {
        var res = await GetLookupsAsync<ConsentResponse>();

        return Ok(res);
    }
    
    [HttpGet("lookups/" + AccountsLookupTypes.Countries)]
    public async Task<ActionResult<IEnumerable<CountryRes>>> GetLookupCountries() {
        var res = await GetLookupsAsync<ContentCountry, CountryRes>();

        return Ok(res);
    }
    
    [HttpGet("lookups/" + AccountsLookupTypes.TaxStatuses)]
    public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupTaxStatuses() {
        var res = await GetLookupsAsync<TaxStatus>();

        return Ok(res);
    }
}
