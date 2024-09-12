using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crm.Models;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
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

    [HttpPut("accounts/find/{emailAddress}")]
    public async Task<IEnumerable<AccountRes>> FindAccountByEmail([FromRoute] string emailAddress) {
        var res = await _accountManager.FindAccountsByEmailAsync(emailAddress);

        return res;
    }

    [HttpPut("accounts/select")]
    public Task<ActionResult> SelectAccountAsync(SelectAccountReq req) {
        // Inject the IAccountAccessor, check that an account has been selected (i.e. accountaccessor does not return null)
        // also check the email on this account matches the member email to guard against cookie tampering. Also more secure
        // to use account ID in cookie rather than reference but should be fine either way.
        //
        // var accountId = _currentAccountAccessor.Get().Id;
        // 
        //_accountManager.UpdateAccountAsync(accountId, req);
        var res = _accountManager.SelectAccount(req.AccountId, req.AccountReference);

        return Task.FromResult<ActionResult>(Ok(res));
    }
}

