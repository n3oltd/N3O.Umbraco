using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Context;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Cart.Queries;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Controllers;

[ApiDocument(CartConstants.ApiName)]
public class CartController : ApiController {
    private readonly IMediator _mediator;
    private readonly CartCookie _cartCookie;

    public CartController(IMediator mediator, CartCookie cartCookie) {
        _mediator = mediator;
        _cartCookie = cartCookie;
    }

    [HttpPost("add")]
    public async Task<ActionResult> Add(AddToCartReq req) {
        var revisionId = await _mediator.SendAsync<AddToCartCommand, AddToCartReq, RevisionId>(req);

        _cartCookie.SetValue(revisionId);
            
        return Ok();
    }

    [HttpPost("upsells/{upsellOfferId:guid}/addToCart")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AddUpsellToCart(AddUpsellToCartReq req) {
        try {
            var revisionId = await _mediator.SendAsync<AddUpsellToCartCommand, AddUpsellToCartReq, RevisionId>(req);

            _cartCookie.SetValue(revisionId);

            return Ok();
        } catch (ResourceNotFoundException ex) {
            return NotFound(ex);
        }
    }
    
    [HttpDelete("bulkRemove")]
    public async Task<ActionResult> BulkRemove(BulkRemoveFromCartReq req) {
        var revisionId = await _mediator.SendAsync<BulkRemoveFromCartCommand, BulkRemoveFromCartReq, RevisionId>(req);
            
        _cartCookie.SetValue(revisionId);

        return Ok();
    }
    
    [HttpGet("summary")]
    public async Task<ActionResult<CartSummaryRes>> GetSummary() {
        var res = await _mediator.SendAsync<GetCartSummaryQuery, None, CartSummaryRes>(None.Empty);

        _cartCookie.SetValue(res.RevisionId);
        
        return Ok(res);
    }
    
    [HttpPut("reset")]
    public async Task<ActionResult> Reset() {
        await _mediator.SendAsync<ResetCartCommand, None, None>(None.Empty);
            
        return Ok();
    }

    [HttpDelete("remove")]
    public async Task<ActionResult> Remove(RemoveFromCartReq req) {
        var revisionId = await _mediator.SendAsync<RemoveFromCartCommand, RemoveFromCartReq, RevisionId>(req);
            
        _cartCookie.SetValue(revisionId);

        return Ok();
    }
    
    [HttpDelete("upsells/{upsellOfferId:guid}/removeFromCart")]
    public async Task<ActionResult> RemoveUpsellFromCart() {
        var revisionId = await _mediator.SendAsync<RemoveUpsellFromCartCommand, None, RevisionId>(None.Empty);

        _cartCookie.SetValue(revisionId);

        return Ok();
    }
}
