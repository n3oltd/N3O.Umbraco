using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.PayPal.Commands;
using N3O.Umbraco.Payments.PayPal.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.PayPal.Controllers;

[ApiDocument(PayPalConstants.ApiName)]
public class PayPalController : ApiController {
    private readonly IMediator _mediator;

    public PayPalController(IMediator mediator) {
        _mediator = mediator;
    }
    
    [HttpPost("payments/{flowId:entityId}/capture")]
    public async Task<ActionResult<PaymentFlowRes<PayPalPayment>>> Capture(PayPalTransactionReq req) {
        var res = await _mediator.SendAsync<CaptureTransactionCommand, PayPalTransactionReq, PaymentFlowRes<PayPalPayment>>(req);

        return Ok(res);
    }
}
