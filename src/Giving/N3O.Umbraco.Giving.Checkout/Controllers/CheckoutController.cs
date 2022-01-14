using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Checkout.Controllers {
    [ResponseCache(CacheProfileName = CacheProfiles.NoCache)]
    [ApiDocument(CheckoutConstants.ApiName)]
    public class CheckoutController : LookupsController<CheckoutLookupsRes> {
        private readonly ILogger<CheckoutController> _logger;
        private readonly IMediator _mediator;

        public CheckoutController(ILogger<CheckoutController> logger,
                              IMediator mediator,
                              ILookups lookups,
                              IUmbracoMapper mapper)
            : base(lookups, mapper) {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("lookups/" + CheckoutLookupTypes.Countries)]
        public async Task<ActionResult<IEnumerable<CountryRes>>> GetLookupCountries() {
            var res = await GetLookupsAsync<Country, CountryRes>();

            return Ok(res);
        }
    }
}
