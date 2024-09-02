using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crm.Models;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crm.Controllers;

[ApiDocument(CrmConstants.ApiName)]
public class CrmController : LookupsController<CrmLookupsRes> {
    private readonly IAccountManager _accountManager;

    public CrmController(ILookups lookups, IUmbracoMapper mapper, IAccountManager accountManager)
        : base(lookups, mapper) {
        _accountManager = accountManager;
    }
    
    // TODO Shagufta Add an update account endpoint here that will go via the account manager
    [HttpPut("accounts/current")]
    public Task<ActionResult> UpdateCurrentAccount(AccountReq req) {
        // Inject the IAccountAccessor, check that an account has been selected (i.e. accountaccessor does not return null)
        // also check the email on this account matches the member email to guard against cookie tampering. Also more secure
        // to use account ID in cookie rather than reference but should be fine either way.
        //
        // var accountId = _currentAccountAccessor.Get().Id;
        // 
        //_accountManager.UpdateAccountAsync(accountId, req);
        
        throw new NotImplementedException();
    }
}