using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.PayPal.Clients.Models;
using N3O.Umbraco.Payments.PayPal.Commands;
using N3O.Umbraco.Payments.PayPal.Models;
using N3O.Umbraco.Payments.PayPal.Models.PayPalCreatePlanRes;
using N3O.Umbraco.Payments.PayPal.Models.PayPalCreatePlanSubscriptionReq;
using N3O.Umbraco.Payments.PayPal.Models.PayPalCredential;
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
    
    [HttpPost("payments/{flowId:entityId}/capturesubscription")]
    public async Task<ActionResult<PaymentFlowRes<PayPalCredential>>> CaptureSubscription(PayPalSubscriptionReq req) {
        var res = await _mediator.SendAsync<CaptureSubscriptionCommand, PayPalSubscriptionReq, PaymentFlowRes<PayPalCredential>>(req);

        return Ok(res);
    }
    
    [HttpPost("credentials/{flowId:entityId}/createplan")]
    public async Task<ActionResult<PayPalCreatePlanRes>> CreatePlan() {
        var res = await _mediator.SendAsync<CreatePlanCommand, None, PayPalCreatePlanRes>(null);

        return Ok(res);
    }
    
    [HttpPost("credentials/{flowId:entityId}/createsubscription")]
    public async Task<ActionResult<Subscription>> CreateSubscription(PayPalCreateSubscriptionReq req) {
        var res = await _mediator.SendAsync<CreateSubscriptionCommand, PayPalCreateSubscriptionReq, PayPalCreateSubscriptionRes>(req);

        return Ok(res);
    }
}
