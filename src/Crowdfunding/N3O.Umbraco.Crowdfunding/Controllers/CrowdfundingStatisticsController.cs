using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content.Settings;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

[ApiDocument(CrowdfundingConstants.StatisticsApiName)]
public class CrowdfundingStatisticsController : ApiController {
    private const string ApiHeaderKey = "Crowdfunding-API-Key";
    
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly Lazy<IMediator> _mediator;
    private readonly Lazy<IWebHostEnvironment> _webHostEnvironment;
    
    public CrowdfundingStatisticsController(Lazy<IContentLocator> contentLocator, Lazy<IMediator> mediator, Lazy<IWebHostEnvironment> webHostEnvironment) {
        _mediator = mediator;
        _webHostEnvironment = webHostEnvironment;
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

    private bool IsAuthorized() {
        var apiKey = Request.Headers[ApiHeaderKey];
        var environment = GetEnvironmentType();
        
        var environmentApiKey = _contentLocator.Value.All<EnvironmentContent>()
                                               .SingleOrDefault(x => x.Environment == environment)?
                                               .ApiKey;

        if (!environmentApiKey.HasValue()) {
            return false;
        }

        return apiKey == environmentApiKey;
    }

    private CrowdfundingEnvironmentType GetEnvironmentType() {
        if (_webHostEnvironment.Value.IsProduction()) {
            return CrowdfundingEnvironmentTypes.Production;
        } else if (_webHostEnvironment.Value.IsStaging() || _webHostEnvironment.Value.IsDevelopment()) {
            return CrowdfundingEnvironmentTypes.Staging;
        } else {
            throw UnrecognisedValueException.For(_webHostEnvironment.Value.EnvironmentName);
        }
    }
}