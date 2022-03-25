using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Mediator.Extensions;
using N3O.Umbraco.Webhooks.Commands;
using System.Threading.Tasks;

namespace N3O.Umbraco.Webhooks.Controllers {
    [ApiDocument(WebhooksConstants.ApiName)]
    public class WebhooksController : ApiController {
        private readonly IMediator _mediator;

        public WebhooksController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpGet("{endpointId}")]
        [HttpGet("{endpointId}/{*endpointRoute}")]
        [HttpPost("{endpointId}")]
        [HttpPost("{endpointId}/{*endpointRoute}")]
        public async Task<ActionResult> Execute() {
            await _mediator.SendAsync<QueueWebhookCommand>();

            return Ok();
        }
    }
}