using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Context;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Cart.Queries;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Controllers {
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
        
        [HttpGet("summary")]
        public async Task<ActionResult<CartSummaryRes>> GetSummary() {
            var res = await _mediator.SendAsync<GetCartSummaryQuery, None, CartSummaryRes>(None.Empty);

            _cartCookie.SetValue(res.RevisionId);
            
            return Ok(res);
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
    }
}
