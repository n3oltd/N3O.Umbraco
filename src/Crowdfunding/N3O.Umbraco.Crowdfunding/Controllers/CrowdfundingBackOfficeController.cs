using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content.Settings;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

[ApiDocument(CrowdfundingConstants.BackOfficeApiName)]
public class CrowdfundingBackOfficeController : BackofficeAuthorizedApiController {
    private readonly IContentLocator _contentLocator;

    public CrowdfundingBackOfficeController(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }

    [HttpGet("Environments")]
    public async Task<ActionResult<IEnumerable<CrowdfundingEnvironmentRes>>> GetCrowdfundingEnvironments() {
        var environments = _contentLocator.All<EnvironmentContent>();
        var res = environments.Select(x => new CrowdfundingEnvironmentRes(x));
        
        return Ok(res);
    }
}