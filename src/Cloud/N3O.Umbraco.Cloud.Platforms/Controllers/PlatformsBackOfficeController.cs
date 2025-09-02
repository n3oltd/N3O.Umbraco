using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Queries;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms.Controllers;

[ApiDocument(PlatformsConstants.BackOfficeApiName)]
public class PlatformsBackOfficeController : BackofficeAuthorizedApiController {
    private readonly IMediator _mediator;

    public PlatformsBackOfficeController(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPost("previewHtml/{contentTypeAlias}")]
    public async Task<ActionResult<string>> GetPreviewHtml(Dictionary<string, object> req) {
        var res = await _mediator.SendAsync<GetPreviewHtmlQuery, Dictionary<string, object>, string>(req);
        
        return Ok(res);
    }
}