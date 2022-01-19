using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.TaxRelief.Lookups;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Checkout.Controllers {
    [ApiDocument(CheckoutConstants.ApiName)]
    public class CheckoutController : LookupsController<CheckoutLookupsRes> {
        private readonly IMediator _mediator;

        public CheckoutController(IMediator mediator, ILookups lookups, IUmbracoMapper mapper)
            : base(lookups, mapper) {
            _mediator = mediator;
        }

        [HttpGet("current")]
        public Task<ActionResult<CheckoutRes>> GetCurrentCheckout() {
            //var res = _mediator.SendAsync<GetCheckout, None, CheckoutRes>(None.Empty);
            // return Ok(res);
            
            throw new NotImplementedException();
        }

        [HttpGet("lookups/" + CheckoutLookupTypes.CheckoutStages)]
        public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupCheckoutStages() {
            var res = await GetLookupsAsync<CheckoutStage>();

            return Ok(res);
        }

        [HttpGet("lookups/" + CheckoutLookupTypes.Countries)]
        public async Task<ActionResult<IEnumerable<CountryRes>>> GetLookupCountries() {
            var res = await GetLookupsAsync<Country, CountryRes>();

            return Ok(res);
        }
        
        [HttpGet("lookups/" + CheckoutLookupTypes.TaxStatuses)]
        public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupTaxStatuses() {
            var res = await GetLookupsAsync<TaxStatus>();

            return Ok(res);
        }
    }
}
