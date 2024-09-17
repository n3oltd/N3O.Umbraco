using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Queries;
using System;
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
    public async Task<ActionResult> UpdateProperty([FromRoute] Guid contentId, ContentPropertyReq req) {
        var content = _contentService.Value.GetById(contentId);

        var canEdit = await _fundraiserAccessControl.Value.CanEditAsync(content);

        if (!canEdit) {
            throw new UnauthorizedAccessException();
        }
        
        await _mediator.Value.SendAsync<UpdateContentPropertyCommand, ContentPropertyReq>(req);

        return Ok();
    }
}