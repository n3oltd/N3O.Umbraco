using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Criteria;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Controller {
    [ApiDocument(PaymentsConstants.ApiName)]
    public class PaymentsController : LookupsController<PaymentsLookupsRes> {
        private readonly IMediator _mediator;

        public PaymentsController(ILookups lookups, IUmbracoMapper mapper, IMediator mediator)
            : base(lookups, mapper) {
            _mediator = mediator;
        }

        [HttpPost("paymentMethods/find")]
        public async Task<ActionResult<IEnumerable<PaymentMethodRes>>> FindPaymentMethods(PaymentMethodCriteria req) {
            var res = await _mediator.SendAsync<FindPaymentMethodsQuery, PaymentMethodCriteria, IEnumerable<PaymentMethodRes>>(req);

            return Ok(res);
        }

        [HttpGet("lookups/" + PaymentsLookupTypes.PaymentMethods)]
        public async Task<ActionResult<IEnumerable<PaymentMethodRes>>> GetLookupPaymentMethods() {
            var res = await GetLookupsAsync<PaymentMethod, PaymentMethodRes>();

            return Ok(res);
        }
    }
}