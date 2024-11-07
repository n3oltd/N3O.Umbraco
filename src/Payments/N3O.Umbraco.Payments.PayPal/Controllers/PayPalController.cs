using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.PayPal.Clients;
using N3O.Umbraco.Payments.PayPal.Commands;
using N3O.Umbraco.Payments.PayPal.Models;
using N3O.Umbraco.Payments.PayPal.Models.PayPalCreatePlanRes;
using N3O.Umbraco.Payments.PayPal.Models.PayPalCreatePlanSubscriptionReq;
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
    
    [HttpPost("credentials/{flowId:entityId}/createsubscription")]
    public async Task<ActionResult<Subscription>> CreateSubscription(PayPalCreateSubscriptionReq req) {
        var res = await _mediator.SendAsync<CreateSubscriptionCommand, PayPalCreateSubscriptionReq, PayPalCreateSubscriptionRes>(req);

        return Ok(res);
    }
    
    [HttpPost("credentials/{flowId:entityId}/createplan")]
    public async Task<ActionResult<PayPalCreatePlanRes>> CreatePlan() {
        var res = await _mediator.SendAsync<CreatePlanCommand, None, PayPalCreatePlanRes>(null);

        return Ok(res);
    }
    
    /*[HttpGet("credentials/{flowId:entityId}/redirectFlow/complete")]
    public async Task<RedirectResult> CompleteRedirectFlow() {
        var res = await _mediator.SendAsync<CompleteRedirectFlowCommand, None, PaymentFlowRes<GoCardlessCredential>>(None.Empty);

        return Redirect(res.Result.ReturnUrl);
    }*/
}
