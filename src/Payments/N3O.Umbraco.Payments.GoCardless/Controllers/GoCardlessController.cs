using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.GoCardless.Commands;
using N3O.Umbraco.Payments.GoCardless.Models;
using N3O.Umbraco.Payments.Models;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.GoCardless.Controllers {
    [ApiDocument(GoCardlessConstants.ApiName)]
    public class GoCardlessController : ApiController {
        private readonly IMediator _mediator;

        public GoCardlessController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpPost("credentials/{flowId:entityId}/begin")]
        public async Task<ActionResult> Begin() {
            await _mediator.SendAsync<BeginRedirectFlowCommand, None, PaymentFlowRes<GoCardlessCredential>>(None.Empty);

            return Ok();
        }

        // TODO Has api in route at the moment
        [HttpGet("credentials/{flowId:entityId}/complete")]
        public async Task<ActionResult> Complete() {
            await _mediator.SendAsync<CompleteRedirectFlowCommand, None, PaymentFlowRes<GoCardlessCredential>>(None.Empty);

            throw new NotImplementedException();
        }
    }
}