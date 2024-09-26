using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crm.Models;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;
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
    
    [HttpPost("accounts")]
    public async Task<string> CreateAccount(AccountReq req) {
        var res = await _accountManager.CreateAccountAsync(req);

        return res;
    }

    [HttpPut("accounts")]
    public async Task<ActionResult> UpdateAccount(AccountReq req) {
        await _accountManager.UpdateAccountAsync(req);

        return Ok();
    }

    [HttpGet("accounts/createdStatus/{accountId}")]
    public async Task<ActionResult<AccountRes>> CreatedStatus([FromRoute] string accountId) {
        var res = await _accountManager.CheckCreatedStatusAsync(accountId);

        return res;
    }
    
}