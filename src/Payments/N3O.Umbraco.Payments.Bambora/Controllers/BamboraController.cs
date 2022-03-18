using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Payments.Bambora.Controllers {
    [ApiDocument(BamboraConstants.ApiName)]
    public class BamboraController : ApiController {
        private readonly IMediator _mediator;

        public BamboraController(IMediator mediator) {
            _mediator = mediator;
        }
    }
}