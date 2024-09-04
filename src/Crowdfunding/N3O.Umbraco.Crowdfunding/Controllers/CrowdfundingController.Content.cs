using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

public partial class CrowdfundingController {
    [HttpGet("content/{contentId:guid}/properties/{propertyAlias}")]
    public async Task<ActionResult<ContentPropertyValueRes>> GetContentPropertyValue() {
        var res = await _mediator.Value.SendAsync<GetContentPropertyValueQuery, None, ContentPropertyValueRes>(None.Empty);

        return Ok(res);
    }
    
    [HttpGet("content/{contentId:guid}/nested/{propertyAlias}/schema")]
    public async Task<ActionResult<NestedSchemaRes>> GetNestedPropertySchema() {
        var res = await _mediator.Value.SendAsync<GetNestedPropertySchemaQuery, None, NestedSchemaRes>(None.Empty);

        return Ok(res);
    }
    
    [HttpPut("content/{contentId:guid}/property")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateProperty(ContentPropertyReq req) {
        await _mediator.Value.SendAsync<UpdateContentPropertyCommand, ContentPropertyReq>(req);

        return Ok();
    }
}