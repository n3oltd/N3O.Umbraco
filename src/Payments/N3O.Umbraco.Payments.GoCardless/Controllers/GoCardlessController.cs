using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.GoCardless.Commands;
using N3O.Umbraco.Payments.GoCardless.Models;
using N3O.Umbraco.Payments.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.GoCardless.Controllers {
    [ApiDocument(GoCardlessConstants.ApiName)]
    public class GoCardlessController : ApiController {
        private readonly IMediator _mediator;

        public GoCardlessController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpPost("credentials/{flowId:entityId}/redirectFlow/begin")]
        public async Task<ActionResult<PaymentFlowRes<GoCardlessCredential>>> BeginRedirectFlow(RedirectFlowReq req) {
            var res = await _mediator.SendAsync<BeginRedirectFlowCommand, RedirectFlowReq, PaymentFlowRes<GoCardlessCredential>>(req);

            return Ok(res);
        }

        [HttpGet("credentials/{flowId:entityId}/redirectFlow/complete")]
        public async Task<RedirectResult> CompleteRedirectFlow() {
            var res = await _mediator.SendAsync<CompleteRedirectFlowCommand, None, PaymentFlowRes<GoCardlessCredential>>(None.Empty);

            return Redirect(res.Result.ReturnUrl);
        }
    }
}