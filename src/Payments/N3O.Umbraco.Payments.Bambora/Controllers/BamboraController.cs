using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Bambora.Commands;
using N3O.Umbraco.Payments.Bambora.Models;
using N3O.Umbraco.Payments.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Bambora.Controllers {
    [ApiDocument(BamboraConstants.ApiName)]
    public class BamboraController : ApiController {
        private readonly IMediator _mediator;

        public BamboraController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpPost("payments/{flowId:entityId}/charge")]
        public async Task<ActionResult<PaymentFlowRes<BamboraPayment>>> ChargeCard(ChargeCardReq req) {
            var res = await _mediator.SendAsync<ChargeCardCommand, ChargeCardReq, PaymentFlowRes<BamboraPayment>>(req);

            return Ok(res);
        }
    }
}