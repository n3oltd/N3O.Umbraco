using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

public partial class CrowdfundingController {
    [HttpPost("statistics/dashboard")]
    public async Task<ActionResult<DashboardStatisticsRes>> GetDashboardStatistics(DashboardStatisticsCriteria req) {
        var res = await _mediator.Value.SendAsync<GetDashboardStatisticsQuery, DashboardStatisticsCriteria, DashboardStatisticsRes>(req);

        return Ok(res);
    }
}