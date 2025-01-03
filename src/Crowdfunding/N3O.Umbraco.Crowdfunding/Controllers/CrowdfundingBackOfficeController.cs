using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content.Settings;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Utilities;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Controllers;

[ApiDocument(CrowdfundingConstants.BackOfficeApiName)]
public class CrowdfundingBackOfficeController : BackofficeAuthorizedApiController {
    private readonly IContentLocator _contentLocator;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CrowdfundingBackOfficeController(IContentLocator contentLocator, IWebHostEnvironment webHostEnvironment) {
        _contentLocator = contentLocator;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet("environments")]
    public ActionResult<IEnumerable<CrowdfundingEnvironmentRes>> GetCrowdfundingEnvironments() {
        var crowdfundingSettings = _contentLocator.Single<SettingsContent>();
        var urlSettings = _contentLocator.Single<UrlSettingsContent>();

        var res = new List<CrowdfundingEnvironmentRes>();
        
        res.Add(CrowdfundingEnvironmentRes.For("Production",
                                               crowdfundingSettings.ApiKey,
                                               urlSettings.ProductionBaseUrl,
                                               true));
        
        res.Add(CrowdfundingEnvironmentRes.For("Staging",
                                               crowdfundingSettings.ApiKey,
                                               urlSettings.StagingBaseUrl,
                                               false));

        if (_webHostEnvironment.IsDevelopment()) {
            res.Add(CrowdfundingEnvironmentRes.For("Development",
                                                   crowdfundingSettings.ApiKey,
                                                   urlSettings.DevelopmentBaseUrl,
                                                   false));
        }
        
        return Ok(res);
    }
}