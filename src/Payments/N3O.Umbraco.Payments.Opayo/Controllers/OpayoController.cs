using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Controller;
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

        [HttpPost("{flowId:guid}/payment/process")]
        public async Task<ActionResult<OpayoPaymentRes>> Process(OpayoPaymentReq req) {
            var res = await _mediator.SendAsync<ProcessPaymentCommand, OpayoPaymentReq, OpayoPaymentRes>(req);

            return Ok(res);
        }

        [HttpPost("callback3ds")]
        public async Task<ActionResult> Callback3ds(ThreeDSecureChallengeReq req) {
            await _mediator.SendAsync<ThreeDSecureChallengeCommand, ThreeDSecureChallengeReq, None>(req);

            return Ok();
        }

        [HttpGet("3DSecureStatus/{transactionId}")]
        public async Task<ActionResult<ThreeDSecureStatus>> ThreeDSecureStatus(string transactionId) {
            var res = await _mediator.SendAsync<GetThreeDSecureChallengeStatusCommand, string, ThreeDSecureStatus>(transactionId);

            return Ok(res);
        }
    }
}