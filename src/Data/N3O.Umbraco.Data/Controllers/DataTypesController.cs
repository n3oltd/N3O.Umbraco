using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Data.Criteria;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data.Controllers;

// TODO Add authentication to this controller
[ApiDocument(DataConstants.ApiNames.DataTypes)]
public class DataTypesController : ApiController {
    private readonly IMediator _mediator;

    public DataTypesController(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPost("find")]
    public async Task<ActionResult<IEnumerable<DataTypeSummary>>> FindDataTypes(DataTypeCriteria req) {
        var res = await _mediator.SendAsync<FindDataTypesQuery, DataTypeCriteria, IEnumerable<DataTypeSummary>>(req);

        return Ok(res);
    }
}
