using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Models;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Controllers;

[ApiDocument(OpayoConstants.ApiName)]
public class OpayoController : ApiController {
    private readonly ILogger<OpayoController> _logger;
    private readonly IMediator _mediator;

    public OpayoController(ILogger<OpayoController> logger, IMediator mediator) {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("payments/{flowId:entityId}/charge")]
    public async Task<ActionResult<PaymentFlowRes<OpayoPayment>>> ChargeCard(ChargeCardReq req) {
        var res = await _mediator.SendAsync<ChargeCardCommand, ChargeCardReq, PaymentFlowRes<OpayoPayment>>(req);

        return Ok(res);
    }
    
    [HttpPost("payments/{flowId:entityId}/completeThreeDSecure")]
    public async Task<RedirectResult> CompleteThreeDSecureChallenge([FromForm] CompleteThreeDSecureReq req) {
        var res = await _mediator.SendAsync<CompleteThreeDSecureCommand, CompleteThreeDSecureReq, PaymentFlowRes<OpayoPayment>>(req);

        return Redirect(res.Result.ReturnUrl);
    }
    
    [HttpGet("merchantSessionKey")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<MerchantSessionKeyRes>> GetMerchantSessionKey() {
        try {
            var res = await _mediator.SendAsync<GetMerchantSessionKeyCommand, None, MerchantSessionKeyRes>(None.Empty);

            return Ok(res);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to get merchant session key");
            
            return UnprocessableEntity();
        }
    }

    [HttpPost("credentials/{flowId:entityId}/store")]
    public async Task<ActionResult<PaymentFlowRes<OpayoCredential>>> StoreCard(StoreCardReq req) {
        var res = await _mediator.SendAsync<StoreCardCommand, StoreCardReq, PaymentFlowRes<OpayoCredential>>(req);

        return Ok(res);
    }
}
