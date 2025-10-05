using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Sync.Extensions.Commands;
using N3O.Umbraco.Sync.Extensions.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Sync.Extensions.Controllers;

[ApiDocument(SyncExtensionsConstants.ApiName)]
public class SyncExtensionsController : ApiController {
    private readonly IMediator _mediator;

    public SyncExtensionsController(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPost("{providerId}/syncData")]
    public async Task<ActionResult> SyncData(SyncDataReq req) {
        var res = await _mediator.SendAsync<SyncDataCommand, SyncDataReq>(req);

        return Ok(res);
    }
}