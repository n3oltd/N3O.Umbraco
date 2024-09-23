using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

public partial class CrowdfundingController {
    [HttpGet("campaigns/{campaignId:guid}/goalOptions/{goalOptionId:guid}")]
    public async Task<ActionResult<GoalOptionRes>> GetCampaignGoalOptions() {
        var res = await _mediator.Value.SendAsync<GetCampaignGoalOptionByIdQuery, None, GoalOptionRes>(None.Empty);

        return Ok(res);
    }
}