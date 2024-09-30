using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Models.AddToCart;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Cart.Context;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

[ApiDocument(CrowdfundingConstants.ApiName)]
public class CrowdfundingCartController : ApiController {
    private readonly Lazy<IMediator> _mediator;
    private readonly CartCookie _cartCookie;

    public CrowdfundingCartController(Lazy<IMediator> mediator,
                                      CartCookie cartCookie) {
        _mediator = mediator;
        _cartCookie = cartCookie;
    }
    
    [HttpPost("Add")]
    public async Task<ActionResult> Add(AddToCartReq req) {
        var revisionId = await _mediator.Value.SendAsync<AddToCartCommand, AddToCartReq, RevisionId>(req);

        _cartCookie.SetValue(revisionId);
            
        return Ok();
    }
}