using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Hosting;

public abstract class WorkspaceVisibilityController : BackofficeAuthorizedApiController {
    [HttpGet("{contentId:guid}")]
    public async Task<ActionResult<WorkspaceVisibilityRes>> GetVisibility([FromRoute] Guid contentId) {
        var res = new WorkspaceVisibilityRes();
        res.Visible = await IsVisibleAsync(contentId);
        
        return Ok(res);
    }

    protected abstract Task<bool> IsVisibleAsync(Guid contentId);
}
