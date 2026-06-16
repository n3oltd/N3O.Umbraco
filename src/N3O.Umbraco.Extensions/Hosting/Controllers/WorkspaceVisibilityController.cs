using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Hosting;

public abstract class WorkspaceVisibilityController : BackofficeAuthorizedApiController {
    [HttpGet("{contentId:guid}")]
    public async Task<ActionResult<WorkspaceVisibilityRes>> GetVisibility([FromRoute] Guid contentId) {
        return Ok(new WorkspaceVisibilityRes { Visible = await IsVisibleAsync(contentId) });
    }

    protected abstract Task<bool> IsVisibleAsync(Guid contentId);
}
