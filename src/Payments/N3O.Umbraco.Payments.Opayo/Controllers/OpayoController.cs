using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Controller;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Opayo.Controllers {
    [ApiDocument(OpayoConstants.ApiName)]
    public class OpayoController : PaymentsController {
        private readonly IMediator _mediator;

        public OpayoController(IMediator mediator, ILookups lookups, IUmbracoMapper mapper)
            : base(lookups, mapper, mediator) {
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

        [HttpPost("{flowId:entityId}/authorize")]
        public async Task<ActionResult<ThreeDSecureStatus>> Authorize([FromForm]ThreeDSecureChallengeReq req) {
            var res = await _mediator.SendAsync<ThreeDSecureChallengeCommand, ThreeDSecureChallengeReq, PaymentFlowRes<OpayoPayment>>(req);

            return Redirect(res.Result.CallbackUrl);
        }
    }
}