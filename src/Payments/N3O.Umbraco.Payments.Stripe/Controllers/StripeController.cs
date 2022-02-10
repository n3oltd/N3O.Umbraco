using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Stripe.Commands;
using N3O.Umbraco.Payments.Stripe.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Stripe.Controllers {
    [ApiDocument(StripeConstants.ApiName)]
    public class StripeController : ApiController {
        private readonly IMediator _mediator;

        public StripeController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpPost("payments/{flowId:entityId}/paymentIntent")]
        public async Task<ActionResult<PaymentFlowRes<StripePayment>>> CreatePaymentIntent(PaymentIntentReq req) {
            await _mediator.SendAsync<CreatePaymentIntentCommand, PaymentIntentReq, PaymentFlowRes<StripePayment>>(req);

            return Ok();
        }
        
        [HttpPost("credentials/{flowId:entityId}/setupIntent")]
        public async Task<ActionResult<PaymentFlowRes<StripeCredential>>> CreateSetupIntent(SetupIntentReq req) {
            await _mediator.SendAsync<CreateSetupIntentCommand, SetupIntentReq, PaymentFlowRes<StripeCredential>>(req);

            return Ok();
        }
        
        [HttpPost("payments/{flowId:entityId}/paymentIntent/confirm")]
        public async Task<ActionResult<PaymentFlowRes<StripePayment>>> ConfirmPaymentIntent() {
            await _mediator.SendAsync<ConfirmPaymentIntentCommand, None, PaymentFlowRes<StripePayment>>(None.Empty);

            return Ok();
        }
        
        [HttpPost("credentials/{flowId:entityId}/setupIntent/confirm")]
        public async Task<ActionResult<PaymentFlowRes<StripeCredential>>> ConfirmSetupIntent() {
            await _mediator.SendAsync<ConfirmSetupIntentCommand, None, PaymentFlowRes<StripeCredential>>(None.Empty);

            return Ok();
        }
    }
}