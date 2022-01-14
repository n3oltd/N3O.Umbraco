using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Hosting {
    [ApiDocument("Lookups")]
    public abstract class LookupsController<TLookupsRes> : ApiController where TLookupsRes : LookupsRes, new() {
        private readonly ILookups _lookups;
        private readonly IUmbracoMapper _mapper;

        protected LookupsController(ILookups lookups, IUmbracoMapper mapper) {
            _lookups = lookups;
            _mapper = mapper;
        }
        
        [HttpPost("lookups/all")]
        public async Task<ActionResult<TLookupsRes>> GetAllLookups(LookupsCriteria criteria) {
            var getLookups = new GetLookups<TLookupsRes>(_lookups, _mapper);
            var res = await getLookups.RunAsync(criteria);

            return Ok(res);
        }
        
        protected async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupsAsync<T>() where T : INamedLookup {
            var listLookups = new ListLookups<T>(_lookups, _mapper);
            var res = await listLookups.RunAsync();

            return Ok(res);
        }
        
        protected async Task<ActionResult<IEnumerable<TRes>>> GetLookupsAsync<TLookup, TRes>()
            where TLookup : INamedLookup
            where TRes : LookupRes {
            var listCustomLookups = new ListCustomLookups<TLookup, TRes>(_lookups, _mapper);
            var res = await listCustomLookups.RunAsync();

            return Ok(res);
        }
    }
}