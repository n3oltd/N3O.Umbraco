using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Controller {
    [ApiDocument(AllocationConstants.ApiName)]
    public class AllocationsController : LookupsController<AllocationsLookupsRes> {
        public AllocationsController(ILookups lookups, IUmbracoMapper mapper) : base(lookups, mapper) { }

        [HttpGet("lookups/" + AllocationsLookupTypes.AllocationTypes)]
        public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupAllocationTypes() {
            var res = await GetLookupsAsync<AllocationType, NamedLookupRes>();

            return Ok(res);
        }
        
        [HttpGet("lookups/" + AllocationsLookupTypes.DonationItems)]
        public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupDonationItems() {
            var res = await GetLookupsAsync<DonationItem, NamedLookupRes>();

            return Ok(res);
        }
        
        [HttpGet("lookups/" + AllocationsLookupTypes.DonationTypes)]
        public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupDonationTypes() {
            var res = await GetLookupsAsync<DonationType, NamedLookupRes>();

            return Ok(res);
        }
        
        [HttpGet("lookups/" + AllocationsLookupTypes.FundDimension1Options)]
        public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupFundDimension1Options() {
            var res = await GetLookupsAsync<FundDimension1Option, NamedLookupRes>();

            return Ok(res);
        }
        
        [HttpGet("lookups/" + AllocationsLookupTypes.FundDimension2Options)]
        public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupFundDimension2Options() {
            var res = await GetLookupsAsync<FundDimension2Option, NamedLookupRes>();

            return Ok(res);
        }
        
        [HttpGet("lookups/" + AllocationsLookupTypes.FundDimension3Options)]
        public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupFundDimension3Options() {
            var res = await GetLookupsAsync<FundDimension3Option, NamedLookupRes>();

            return Ok(res);
        }
        
        [HttpGet("lookups/" + AllocationsLookupTypes.FundDimension4Options)]
        public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupFundDimension4Options() {
            var res = await GetLookupsAsync<FundDimension4Option, NamedLookupRes>();

            return Ok(res);
        }
        
        [HttpGet("lookups/" + AllocationsLookupTypes.SponsorshipSchemes)]
        public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupSponsorshipSchemes() {
            var res = await GetLookupsAsync<SponsorshipScheme, NamedLookupRes>();

            return Ok(res);
        }
    }
}