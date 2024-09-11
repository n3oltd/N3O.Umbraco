using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

public partial class CrowdfundingController {
    [HttpPost("fundraisers/suggestSlug")]
    public async Task<ActionResult<string>> SuggestSlug([FromQuery] string name) {
        var res = await _mediator.Value.SendAsync<SuggestSlugQuery, string, string>(name);

        return Ok(res);
    }
    
    [HttpPost("fundraisers")]
    public async Task<ActionResult<string>> CreateFundraiser(CreateFundraiserReq req) {
        var res = await _mediator.Value.SendAsync<CreateFundraiserCommand, CreateFundraiserReq, string>(req);

        return Ok(res);
    }
    
    [HttpGet("fundraisers/goals/{contentId:guid}")]
    public async Task<ActionResult<FundraiserGoalsRes>> GetFundraiserGoals() {
        var res = await _mediator.Value.SendAsync<GetFundraiserGoalsQuery, None, FundraiserGoalsRes>(None.Empty);

        return Ok(res);
    }
}