using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

public partial class CrowdfundingController {
    [HttpPost("fundraisers/checkTitle")]
    public async Task<ActionResult<bool>> CheckTitle(CreateFundraiserReq req) {
        var res = await _mediator.Value.SendAsync<CheckFundraiserTitleIsAvailableQuery, CreateFundraiserReq, bool>(req);

        return Ok(res);
    }
    
    [HttpPost("fundraisers")]
    public async Task<ActionResult<string>> CreateFundraiser(CreateFundraiserReq req) {
        var res = await _mediator.Value.SendAsync<CreateFundraiserCommand, CreateFundraiserReq, string>(req);

        return Ok(res);
    }
}