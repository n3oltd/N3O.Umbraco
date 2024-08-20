﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

public partial class CrowdfundingController {
    [HttpGet("content/{contentId:guid}/properties/{propertyAlias}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContentPropertyRes>> GetContentPropertyValue() {
        var res = await _mediator.Value.SendAsync<GetContentPropertyValueQuery, None, ContentPropertyRes>(None.Empty);

        return Ok(res);
    }
    
    [HttpPut("content/{contentId:guid}/property")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateProperty(ContentPropertyReq req) {
        await _mediator.Value.SendAsync<UpdateContentPropertyCommand, ContentPropertyReq>(req);

        return Ok();
    }
}