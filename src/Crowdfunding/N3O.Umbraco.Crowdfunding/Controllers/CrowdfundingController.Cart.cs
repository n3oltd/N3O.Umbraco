﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

public partial class CrowdfundingController {
    [AllowAnonymous]
    [HttpPost("addToCart")]
    public async Task<ActionResult> AddToCart(CrowdfundingCartReq crowdfundingReq) {
        var req = crowdfundingReq.ToBulkAddToCartReq(_contentLocator.Value);
        
        var revisionId = await _mediator.Value.SendAsync<BulkAddToCartCommand, BulkAddToCartReq, RevisionId>(req);

        _cartCookie.Value.SetValue(revisionId);
            
        return Ok();
    }
}