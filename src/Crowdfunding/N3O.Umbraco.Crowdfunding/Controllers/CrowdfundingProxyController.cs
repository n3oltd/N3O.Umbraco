using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content.Settings;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

[ApiDocument(CrowdfundingConstants.ProxyApiName)]
public class CrowdfundingProxyController : ApiController {
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly Lazy<IMediator> _mediator;
    
    public CrowdfundingProxyController(Lazy<IContentLocator> contentLocator, Lazy<IMediator> mediator) {
        _mediator = mediator;
        _contentLocator = contentLocator;
    }
    
    [HttpPost("dashboard")]
    public async Task<ActionResult<DashboardStatisticsRes>> GetDashboardStatistics(DashboardStatisticsCriteria req) {
        if (!IsAuthorized()) {
            return Unauthorized();
        }
        
        var res = await _mediator.Value.SendAsync<GetDashboardStatisticsQuery, DashboardStatisticsCriteria, DashboardStatisticsRes>(req);
        
        return Ok(res);
    }
    
    [HttpPost("pages")]
    public async Task<ActionResult<FundraiserPageResultsPage>> GetFundraiserPages(FundraiserPagesCriteria req) {
        if (!IsAuthorized()) {
            return Unauthorized();
        }
        
        var res = await _mediator.Value.SendAsync<GetFundraiserPagesQuery, FundraiserPagesCriteria, FundraiserPageResultsPage>(req);
        
        return Ok(res);
    }

    private bool IsAuthorized() {
        var apiKeyHeader = Request.Headers[CrowdfundingConstants.Http.Headers.ApiHeaderKey];
        var apiKey = _contentLocator.Value.Single<SettingsContent>().ApiKey;
        
        return apiKey == apiKeyHeader;
    }
}