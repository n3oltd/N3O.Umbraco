using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

public partial class CrowdfundingController {
    [HttpGet("lookups/propertyTypes")]
    public async Task<ActionResult<IEnumerable<LookupRes>>> GetCrowdfundingPagePropertyTypes() {
        var listLookups = new ListCustomLookups<PropertyType, LookupRes>(_lookups.Value, _mapper.Value);
        var res = await listLookups.RunAsync();

        return Ok(res);
    }
}