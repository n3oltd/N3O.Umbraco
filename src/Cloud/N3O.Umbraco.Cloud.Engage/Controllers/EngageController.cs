using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Engage.Controllers;

[ApiDocument(EngageConstants.ApiName)]
public class EngageController : ApiController {
    private readonly IAccountManager _accountManager;

    public EngageController(IAccountManager accountManager) {
        _accountManager = accountManager;
    }
    
    [HttpGet("accounts/{accountId}/checkCreatedStatus")]
    public async Task<ActionResult<AccountRes>> CheckCreatedStatus([FromRoute] string accountId) {
        var res = await _accountManager.CheckCreatedStatusAsync(accountId);

        if (res.IsCreated) {
            return Ok(res);
        } else {
            return NotFound();
        }
    }
    
    [HttpPost("accounts")]
    public async Task<ActionResult<string>> CreateAccount(AccountReq req) {
        var res = await _accountManager.CreateAccountAsync(req);

        return Ok(res);
    }

    [HttpPut("accounts")]
    public async Task<ActionResult> UpdateAccount(AccountReq req) {
        await _accountManager.UpdateAccountAsync(req);

        return Ok();
    }
}