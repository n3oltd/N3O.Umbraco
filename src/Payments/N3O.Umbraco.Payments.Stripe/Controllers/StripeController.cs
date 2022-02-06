using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Payments.Stripe.Controllers {
    [ApiDocument(StripeConstants.ApiName)]
    public class StripeController : ApiController {
        private readonly IMediator _mediator;

        public StripeController(IMediator mediator) {
            _mediator = mediator;
        }

        public ActionResult None() => Ok();
    }
}