using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Cart.Context;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using Read.Core.Commands;
using Read.Core.Models;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Controllers;

[ApiDocument(UpsellConstants.ApiName)]
public class UpsellsController : ApiController {
    private readonly ILogger<CartController> _logger;
    private readonly IMediator _mediator;
    private readonly CartCookie _cartCookie;
    public UpsellsController(ILogger<CartController> logger, IMediator mediator, CartCookie cartCookie) {
        _logger = logger;
        _mediator = mediator;
        _cartCookie = cartCookie;
    }

    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> Add(AddUpsellToCartReq toCartReq) {
        try {
            var revisionId = await _mediator.SendAsync<AddUpsellItemToCartCommand, AddUpsellToCartReq, RevisionId>(toCartReq);

            _cartCookie.SetValue(revisionId);
            
            return Ok();
        } catch (Exception ex) {
            _logger.LogError(ex, ex.Message);
            
            return UnprocessableEntity();
        }
    }
}