using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;
using N3O.Umbraco.Scheduler.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Scheduler.Controllers;

[ApiDocument("JobProxy")]
public class JobProxyController : ApiController {
    private readonly IJsonProvider _jsonProvider;
    private readonly IMediator _mediator;
    private readonly IFluentParameters _fluentParameters;

    public JobProxyController(IJsonProvider jsonProvider, IMediator mediator, IFluentParameters fluentParameters) {
        _jsonProvider = jsonProvider;
        _mediator = mediator;
        _fluentParameters = fluentParameters;
    }

    [HttpPost("executeProxied")]
    public async Task<ActionResult> ExecuteProxiedAsync(ProxyReq req) {
        var model = _jsonProvider.DeserializeObject(req.RequestBody, req.RequestType);

        foreach (var (name, value) in req.ParameterData.OrEmpty()) {
            _fluentParameters.Add(name, value);
        }
                    
        await _mediator.SendAsync(req.CommandType, typeof(None), model);
        
        return Ok();
    }
}