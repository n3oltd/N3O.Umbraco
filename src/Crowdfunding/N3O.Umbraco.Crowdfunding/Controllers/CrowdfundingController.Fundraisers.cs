using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

public partial class CrowdfundingController {
    [HttpPost("fundraisers/{fundraiserId:guid}/activate")]
    public async Task<ActionResult> ActivateFundraiser([FromRoute] Guid fundraiserId) {
        await EnforceFundraiserAccessControlsAsync(fundraiserId);
        
        await _mediator.Value.SendAsync<ActivateFundraiserCommand, None>(None.Empty);
        
        return Ok();
    }
    
    [HttpPost("fundraisers")]
    public async Task<ActionResult<string>> CreateFundraiser(CreateFundraiserReq req) {
        var res = await _mediator.Value.SendAsync<CreateFundraiserCommand, CreateFundraiserReq, string>(req);

        return Ok(res);
    }
    
    [HttpPost("fundraisers/{fundraiserId:guid}/deactivate")]
    public async Task<ActionResult> DeactivateFundraiser([FromRoute] Guid fundraiserId) {
        await EnforceFundraiserAccessControlsAsync(fundraiserId);
        
        await _mediator.Value.SendAsync<ActivateFundraiserCommand, None>(None.Empty);
        
        return Ok();
    }
    
    [HttpGet("fundraisers/{fundraiserId:guid}/goals")]
    public async Task<ActionResult<FundraiserGoalsRes>> GetFundraiserGoals() {
        var res = await _mediator.Value.SendAsync<GetFundraiserGoalsQuery, None, FundraiserGoalsRes>(None.Empty);

        return Ok(res);
    }
    
    
    [HttpPut("fundraisers/{fundraiserId:guid}/goals")]
    public async Task<ActionResult> UpdateFundraiserGoals([FromRoute] Guid fundraiserId, FundraiserGoalsReq req) {
        await EnforceFundraiserAccessControlsAsync(fundraiserId);
        
        var res = await _mediator.Value.SendAsync<UpdateFundraiserGoalsCommand, FundraiserGoalsReq>(req);

        return Ok(res);
    }
}