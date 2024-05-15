using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.TotalProcessing.Commands;
using N3O.Umbraco.Payments.TotalProcessing.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.TotalProcessing.Controllers;

[ApiDocument(TotalProcessingConstants.ApiName)]
public class TotalProcessingController : ApiController {
    private readonly IMediator _mediator;

    public TotalProcessingController(IMediator mediator) {
        _mediator = mediator;
    }
    
    [HttpGet("credentials/{flowId:entityId}/processed")]
    public async Task<ActionResult> CredentialProcessed([FromQuery] CheckoutCompletedReq req) {
        var res = await _mediator.SendAsync<CredentialProcessedCommand, CheckoutCompletedReq,PaymentFlowRes<TotalProcessingCredential>>(req);
            
        return Redirect(res.Result.ReturnUrl);
    }
    
    [HttpGet("payments/{flowId:entityId}/processed")]
    public async Task<ActionResult> PaymentProcessed([FromQuery] CheckoutCompletedReq req) {
        var res = await _mediator.SendAsync<PaymentProcessedCommand, CheckoutCompletedReq,PaymentFlowRes<TotalProcessingPayment>>(req);
            
        return Redirect(res.Result.ReturnUrl);
    }
    
    [HttpPost("credentials/{flowId:entityId}/checkout")]
    public async Task<ActionResult<TotalProcessingCredential>> PrepareCredentialCheckout(PrepareCheckoutReq req) {
        var res = await _mediator.SendAsync<PrepareCredentialCheckoutCommand, PrepareCheckoutReq, PaymentFlowRes<TotalProcessingCredential>>(req);

        return Ok(res);
    }

    [HttpPost("payments/{flowId:entityId}/checkout")]
    public async Task<ActionResult<TotalProcessingPayment>> PreparePaymentCheckout(PrepareCheckoutReq req) {
        var res = await _mediator.SendAsync<PreparePaymentCheckoutCommand, PrepareCheckoutReq, PaymentFlowRes<TotalProcessingPayment>>(req);

        return Ok(res);
    }
}
