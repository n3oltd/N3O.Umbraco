using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

public partial class CrowdfundingController {
    [HttpGet("contents/{contentId:guid}/properties/{propertyAlias}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContentPropertyValueRes>> GetContentPropertyValue() {
        var res = await _mediator.Value.SendAsync<GetContentPropertyValueQuery, None, ContentPropertyValueRes>(None.Empty);

        return Ok(res);
    }
    
    [HttpPut("contents/{contentId:guid}/property")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateProperty(ContentPropertyReq req) {
        await _mediator.Value.SendAsync<UpdateContentPropertyCommand, ContentPropertyReq>(req);

        return Ok();
    }
}