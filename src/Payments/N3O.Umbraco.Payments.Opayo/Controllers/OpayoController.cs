using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Controllers {
    [ApiDocument(OpayoConstants.ApiName)]
    public class OpayoController : ApiController {
        private readonly IMediator _mediator;

        public OpayoController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpGet("merchantSessionKey")]
        public async Task<ActionResult> GetMerchantSessionKey() {
            var res = await _mediator.SendAsync<GetMerchantSessionKeyCommand, None, MerchantSessionKeyRes>(None.Empty);

            return Ok(res);
        }
        
        [HttpPost("{flowId:entityId}/payment/process")]
        public async Task<ActionResult<PaymentFlowRes<OpayoPayment>>> Process(OpayoPaymentReq req) {
            var res = await _mediator.SendAsync<ProcessPaymentCommand, OpayoPaymentReq, PaymentFlowRes<OpayoPayment>>(req);

            return Ok(res);
        }

        [HttpPost("{flowId:entityId}/completeThreeDSecureChallenge")]
        public async Task<ActionResult> CompleteThreeDSecureChallenge([FromForm] ThreeDSecureChallengeReq req) {
            var res = await _mediator.SendAsync<CompleteThreeDSecureChallengeCommand, ThreeDSecureChallengeReq, PaymentFlowRes<OpayoPayment>>(req);

            return Redirect(res.Result.ReturnUrl);
        }
    }
}