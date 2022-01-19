using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Mediator.Extensions;
using N3O.Umbraco.Payments.Controller;
using N3O.Umbraco.Payments.GoCardless.Commands;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.GoCardless.Controllers {
    public class GoCardlessCredentialsController : ApiController {
        private readonly IMediator _mediator;

        public GoCardlessCredentialsController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpPost("credentials/{flowId:guid}/begin")]
        public async Task<ActionResult> Begin() {
            await _mediator.SendAsync<BeginRedirectFlowCommand>();

            return Ok();
        }

        // TODO Has api in route at the moment
        [HttpGet("credentials/{flowId:guid}/complete")]
        public async Task<ActionResult> Complete() {
            await _mediator.SendAsync<CompleteRedirectFlowCommand>();

            throw new NotImplementedException();
        }
    }
}