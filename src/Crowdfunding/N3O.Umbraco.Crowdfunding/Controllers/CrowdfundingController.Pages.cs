using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

public partial class CrowdfundingController {
    [HttpPost("pages/checkName")]
    public async Task<ActionResult<bool>> CheckName(CreatePageReq req) {
        var res = await _mediator.Value.SendAsync<CheckPageNameIsAvailableQuery, CreatePageReq, bool>(req);

        return Ok(res);
    }
    
    [HttpPost("pages")]
    public async Task<ActionResult<string>> CreatePage(CreatePageReq req) {
        var res = await _mediator.Value.SendAsync<CreatePageCommand, CreatePageReq, string>(req);

        return Ok(res);
    }
    
    [HttpGet("pages/{pageId:guid}/properties/{propertyAlias}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PagePropertyValueRes>> GetPagePropertyValue() {
        var res = await _mediator.Value.SendAsync<GetPagePropertyValueQuery, None, PagePropertyValueRes>(None.Empty);

        return Ok(res);
    }
    
    [HttpPut("pages/{pageId:guid}/property")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateProperty(PagePropertyReq req) {
        await _mediator.Value.SendAsync<UpdatePagePropertyCommand, PagePropertyReq>(req);

        return Ok();
    }
}