using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.FundDimensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Allocations.Queries;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Controller {
    [ApiDocument(AllocationsConstants.ApiName)]
    public class AllocationsController : LookupsController<AllocationsLookupRes> {
        private readonly IMediator _mediator;

        public AllocationsController(ILookups lookups, IUmbracoMapper mapper, IMediator mediator)
            : base(lookups, mapper) {
            _mediator = mediator;
        }

        [HttpGet("fundStructure")]
        public async Task<ActionResult<FundStructure>> GetFundStructure() {
            var res = await _mediator.SendAsync<GetFundStructureQuery, None, FundStructureRes>(None.Empty);

            return Ok(res);
        }
        
        [HttpGet("lookups/" + AllocationsLookupTypes.AllocationTypes)]
        public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupAllocationTypes() {
            var res = await GetLookupsAsync<AllocationType>();

            return Ok(res);
        }
        
        [HttpGet("lookups/" + AllocationsLookupTypes.DonationItems)]
        public async Task<ActionResult<IEnumerable<DonationItemRes>>> GetLookupDonationItems() {
            var res = await GetLookupsAsync<DonationItem, DonationItemRes>();

            return Ok(res);
        }

        [HttpGet("lookups/" + AllocationsLookupTypes.FundDimension1Options)]
        public async Task<ActionResult<IEnumerable<FundDimensionOptionRes>>> GetLookupFundDimension1Options() {
            var res = await GetLookupsAsync<FundDimension1Option, FundDimensionOptionRes>();

            return Ok(res);
        }
        
        [HttpGet("lookups/" + AllocationsLookupTypes.FundDimension2Options)]
        public async Task<ActionResult<IEnumerable<FundDimensionOptionRes>>> GetLookupFundDimension2Options() {
            var res = await GetLookupsAsync<FundDimension2Option, FundDimensionOptionRes>();

            return Ok(res);
        }
        
        [HttpGet("lookups/" + AllocationsLookupTypes.FundDimension3Options)]
        public async Task<ActionResult<IEnumerable<FundDimensionOptionRes>>> GetLookupFundDimension3Options() {
            var res = await GetLookupsAsync<FundDimension3Option, FundDimensionOptionRes>();

            return Ok(res);
        }
        
        [HttpGet("lookups/" + AllocationsLookupTypes.FundDimension4Options)]
        public async Task<ActionResult<IEnumerable<FundDimensionOptionRes>>> GetLookupFundDimension4Options() {
            var res = await GetLookupsAsync<FundDimension4Option, FundDimensionOptionRes>();

            return Ok(res);
        }
        
        [HttpGet("lookups/" + AllocationsLookupTypes.GivingTypes)]
        public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupGivingTypes() {
            var res = await GetLookupsAsync<GivingType>();

            return Ok(res);
        }
    }
}