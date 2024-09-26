using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

public partial class CrowdfundingController {
    [HttpPost("fundraisers")]
    public async Task<ActionResult<string>> CreateFundraiser(CreateFundraiserReq req) {
        var res = await _mediator.Value.SendAsync<CreateFundraiserCommand, CreateFundraiserReq, string>(req);

        return Ok(res);
    }
    
    [HttpGet("fundraisers/{contentId:guid}/goals")]
    public async Task<ActionResult<FundraiserGoalsRes>> GetFundraiserGoals() {
        var res = await _mediator.Value.SendAsync<GetFundraiserGoalsQuery, None, FundraiserGoalsRes>(None.Empty);

        return Ok(res);
    }
    
    [HttpPost("fundraisers/{fundraiserId:guid}/publish")]
    public async Task<ActionResult> PublishFundraiser() {
        await _mediator.Value.SendAsync<PublishFundraiserCommand, None, None>(None.Empty);
        
        return Ok();
    }
    
    [HttpPut("fundraisers/{contentId:guid}/goals")]
    public async Task<ActionResult> UpdateFundraiserGoals(FundraiserGoalsReq req) {
        var res = await _mediator.Value.SendAsync<UpdateFundraiserGoalsCommand, FundraiserGoalsReq>(req);

        return Ok(res);
    }
}