using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Hosting;

[ApiDocument("Lookups")]
public abstract class LookupsController<TLookupsRes> : ApiController where TLookupsRes : LookupsRes, new() {
    protected LookupsController(ILookups lookups, IUmbracoMapper mapper) {
        Lookups = lookups;
        Mapper = mapper;
    }
    
    [HttpPost("lookups/all")]
    public async Task<ActionResult<TLookupsRes>> GetAllLookups(LookupsCriteria criteria) {
        var getLookups = new GetLookups<TLookupsRes>(Lookups, Mapper);
        var res = await getLookups.RunAsync(criteria);

        return Ok(res);
    }
    
    protected async Task<IEnumerable<NamedLookupRes>> GetLookupsAsync<T>() where T : INamedLookup {
        var listLookups = new ListLookups<T>(Lookups, Mapper);
        var res = await listLookups.RunAsync();

        return res;
    }
    
    protected async Task<IEnumerable<TRes>> GetLookupsAsync<TLookup, TRes>()
        where TLookup : INamedLookup
        where TRes : LookupRes {
        var listCustomLookups = new ListCustomLookups<TLookup, TRes>(Lookups, Mapper);
        var res = await listCustomLookups.RunAsync();

        return res;
    }
    
    protected ILookups Lookups { get; }
    protected IUmbracoMapper Mapper { get; }
}
