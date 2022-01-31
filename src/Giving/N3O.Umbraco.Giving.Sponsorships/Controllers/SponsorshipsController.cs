using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Giving.Sponsorships.Lookups;
using N3O.Umbraco.Giving.Sponsorships.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Sponsorships.Controller {
    [ApiDocument(SponsorshipsConstants.ApiName)]
    public class SponsorshipsController : LookupsController<SponsorshipsLookupsRes> {
        public SponsorshipsController(ILookups lookups, IUmbracoMapper mapper) : base(lookups, mapper) { }

        [HttpGet("lookups/" + SponsorshipsLookupTypes.SponsorshipDurations)]
        public async Task<ActionResult<IEnumerable<SponsorshipDurationRes>>> GetLookupSponsorshipDurations() {
            var res = await GetLookupsAsync<SponsorshipDuration, SponsorshipDurationRes>();

            return Ok(res);
        }
        
        [HttpGet("lookups/" + SponsorshipsLookupTypes.SponsorshipSchemes)]
        public async Task<ActionResult<IEnumerable<SponsorshipSchemeRes>>> GetLookupSponsorshipSchemes() {
            var res = await GetLookupsAsync<SponsorshipScheme, SponsorshipSchemeRes>();

            return Ok(res);
        }
    }
}