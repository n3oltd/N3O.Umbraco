using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Mediator.Extensions;
using N3O.Umbraco.Payments.Controller;
using N3O.Umbraco.Payments.GoCardless.Commands;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.GoCardless.Controllers {
    public class GoCardlessCredentialsController : PaymentsController {
        private readonly IMediator _mediator;

        public GoCardlessCredentialsController(Lazy<IPaymentsScope> paymentsScope, IMediator mediator)
            : base(paymentsScope) {
            _mediator = mediator;
        }

        [HttpPost("{flowId:guid}/begin")]
        public async Task<ActionResult> Begin() {
            await _mediator.SendAsync<BeginRedirectFlowCommand>();

            return Ok();
        }

        // TODO Has api in route at the moment
        [HttpGet("{flowId:guid}/complete")]
        public async Task<ActionResult> Complete() {
            await _mediator.SendAsync<CompleteRedirectFlowCommand>();

            return await NextAsync();
        }
    }
}