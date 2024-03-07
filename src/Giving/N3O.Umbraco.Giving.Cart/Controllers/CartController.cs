using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Context;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Cart.Queries;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Controllers;

[ApiDocument(CartConstants.ApiName)]
public class CartController : ApiController {
    private readonly ILogger<CartController> _logger;
    private readonly IMediator _mediator;
    private readonly CartCookie _cartCookie;

    public CartController(ILogger<CartController> logger, IMediator mediator, CartCookie cartCookie) {
        _logger = logger;
        _mediator = mediator;
        _cartCookie = cartCookie;
    }

    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> Add(AddToCartReq req) {
        try {
            var revisionId = await _mediator.SendAsync<AddToCartCommand, AddToCartReq, RevisionId>(req);

            _cartCookie.SetValue(revisionId);
            
            return Ok();
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to add to cart for request {@Req}", req);
            
            return UnprocessableEntity();
        }
    }

    [HttpPost("upsells/{upsellOfferId:guid}/addToCart")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AddUpsellToCart(AddUpsellToCartReq req) {
        try {
            var revisionId = await _mediator.SendAsync<AddUpsellToCartCommand, AddUpsellToCartReq, RevisionId>(req);

            _cartCookie.SetValue(revisionId);

            return Ok();
        } catch (ResourceNotFoundException ex) {
            return NotFound(ex);
        } catch (Exception ex) {
            _logger.LogError(ex, ex.Message);

            return UnprocessableEntity();
        }
    }
    
    [HttpDelete("bulkRemove")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> BulkRemove(BulkRemoveFromCartReq req) {
        try { 
            var revisionId = await _mediator.SendAsync<BulkRemoveFromCartCommand, BulkRemoveFromCartReq, RevisionId>(req);
            
            _cartCookie.SetValue(revisionId);

            return Ok();
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to remove items from cart");

            return UnprocessableEntity();
        }
    }
    
    [HttpGet("summary")]
    public async Task<ActionResult<CartSummaryRes>> GetSummary() {
        var res = await _mediator.SendAsync<GetCartSummaryQuery, None, CartSummaryRes>(None.Empty);

        _cartCookie.SetValue(res.RevisionId);
        
        return Ok(res);
    }
    
    [HttpPut("reset")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> Reset() {
        try {
            await _mediator.SendAsync<ResetCartCommand, None, None>(None.Empty);
            
            return Ok();
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to reset cart");

            return UnprocessableEntity();
        }
    }

    [HttpDelete("remove")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> Remove(RemoveFromCartReq req) {
        try {
            var revisionId = await _mediator.SendAsync<RemoveFromCartCommand, RemoveFromCartReq, RevisionId>(req);
            
            _cartCookie.SetValue(revisionId);

            return Ok();
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to remove item from cart");

            return UnprocessableEntity();
        }
    }
    
    [HttpDelete("upsells/{upsellOfferId:guid}/removeFromCart")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> RemoveUpsellFromCart() {
        try {
            var revisionId = await _mediator.SendAsync<RemoveUpsellFromCartCommand, None, RevisionId>(None.Empty);

            _cartCookie.SetValue(revisionId);

            return Ok();
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to remove upsell from cart");

            return UnprocessableEntity();
        }
    }
}
