using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Financial;
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
    
    [HttpPost("payments/{flowId:entityId}/captureSubscription")]
    public async Task<ActionResult<PaymentFlowRes<PayPalCredential>>> CaptureSubscription(PayPalSubscriptionReq req) {
        var res = await _mediator.SendAsync<CaptureSubscriptionCommand, PayPalSubscriptionReq, PaymentFlowRes<PayPalCredential>>(req);

        return Ok(res);
    }
    
    [HttpPost("payments/{flowId:entityId}/captureTransaction")]
    public async Task<ActionResult<PaymentFlowRes<PayPalPayment>>> CaptureTransaction(PayPalTransactionReq req) {
        var res = await _mediator.SendAsync<CaptureTransactionCommand, PayPalTransactionReq, PaymentFlowRes<PayPalPayment>>(req);

        return Ok(res);
    }
    
    [HttpPost("credentials/{flowId:entityId}/getOrCreatePlan")]
    public async Task<ActionResult<string>> GetOrCreatePlan(MoneyReq req) {
        var res = await _mediator.SendAsync<GetOrCreatePlanCommand, MoneyReq, string>(req);

        return Ok(res);
    }
}
