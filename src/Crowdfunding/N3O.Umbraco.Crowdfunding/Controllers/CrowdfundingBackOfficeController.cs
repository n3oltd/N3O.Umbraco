using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content.Settings;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

[ApiDocument(CrowdfundingConstants.BackOfficeApiName)]
public class CrowdfundingBackOfficeController : BackofficeAuthorizedApiController {
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly Lazy<IWebHostEnvironment> _webHostEnvironment;

    public CrowdfundingBackOfficeController(Lazy<IContentLocator> contentLocator,
                                            Lazy<IWebHostEnvironment> webHostEnvironment) {
        _contentLocator = contentLocator;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet("Environments")]
    public async Task<ActionResult<IEnumerable<CrowdfundingEnvironmentRes>>> GetCrowdfundingEnvironments() {
        var environments = _contentLocator.Value.All<EnvironmentContent>();

        if (_webHostEnvironment.Value.IsProduction()) {
            environments = environments.Where(x => x.Environment == CrowdfundingEnvironmentTypes.Production).ToList();
        }
        
        var res = environments.Select(x => new CrowdfundingEnvironmentRes(x));
        
        return Ok(res);
    }
}