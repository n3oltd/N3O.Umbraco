using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.TotalProcessing.Commands;
using N3O.Umbraco.Payments.TotalProcessing.Models;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.TotalProcessing.Controllers;

[ApiDocument(TotalProcessingConstants.ApiName)]
public class TotalProcessingController : ApiController {
    private readonly ILogger<TotalProcessingController> _logger;
    private readonly IMediator _mediator;

    public TotalProcessingController(ILogger<TotalProcessingController> logger, IMediator mediator) {
        _logger = logger;
        _mediator = mediator;
    }
    
    [HttpGet("credentials/{flowId:entityId}/processed")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> CredentialProcessed([FromQuery] CheckoutCompletedReq req) {
        try {
            var res = await _mediator.SendAsync<CredentialProcessedCommand, CheckoutCompletedReq,PaymentFlowRes<TotalProcessingCredential>>(req);
            
            return Redirect(res.Result.ReturnUrl);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to process credential");

            return UnprocessableEntity();
        }
    }
    
    [HttpGet("payments/{flowId:entityId}/processed")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> PaymentProcessed([FromQuery] CheckoutCompletedReq req) {
        try {
            var res = await _mediator.SendAsync<PaymentProcessedCommand, CheckoutCompletedReq,PaymentFlowRes<TotalProcessingPayment>>(req);
            
            return Redirect(res.Result.ReturnUrl);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to process payment");

            return UnprocessableEntity();
        }
    }
    
    [HttpPost("credentials/{flowId:entityId}/checkout")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<TotalProcessingCredential>> PrepareCredentialCheckout(PrepareCheckoutReq req) {
        try {
            var res = await _mediator.SendAsync<PrepareCredentialCheckoutCommand, PrepareCheckoutReq, PaymentFlowRes<TotalProcessingCredential>>(req);

            return Ok(res);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to prepare checkout");

            return UnprocessableEntity();
        }
    }

    [HttpPost("payments/{flowId:entityId}/checkout")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<TotalProcessingPayment>> PreparePaymentCheckout(PrepareCheckoutReq req) {
        try {
            var res = await _mediator.SendAsync<PreparePaymentCheckoutCommand, PrepareCheckoutReq, PaymentFlowRes<TotalProcessingPayment>>(req);

            return Ok(res);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to prepare checkout");

            return UnprocessableEntity();
        }
    }
}
