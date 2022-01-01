using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Cart.Queries;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Mediator.Extensions;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Controllers {
    [ResponseCache(CacheProfileName = CacheProfiles.NoCache)]
    [ApiDocument(CartConstants.ApiName)]
    public class CartController : ApiController {
        private readonly IMediator _mediator;

        public CartController(ILogger<CartController> logger, IMediator mediator) : base(logger) {
            _mediator = mediator;
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> Add(AddToCartReq req) {
            try {
                await _mediator.SendAsync<AddToCartCommand, AddToCartReq>(req);

                return Ok();
            } catch (Exception ex) {
                return RequestFailed(l => l.LogError(ex, "Failed to add to cart for request {Req}", req));
            }
        }
        
        [HttpGet("summary")]
        public async Task<ActionResult<CartSummaryRes>> GetSummary() {
            var res = await _mediator.SendAsync<GetCartSummaryQuery, None, CartSummaryRes>(None.Empty);

            return Ok(res);
        }

        [HttpPost("remove/{allocationNumber:int}")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> Remove() {
            try {
                await _mediator.SendAsync<RemoveFromCartCommand>();

                return Ok();
            } catch (Exception ex) {
                return RequestFailed(l => l.LogError(ex, "Failed to remove item from cart"));
            }
        }
    }
}
