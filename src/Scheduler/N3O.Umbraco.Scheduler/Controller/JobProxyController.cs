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
    
    public JobProxyController(IJsonProvider jsonProvider) {
        _jsonProvider = jsonProvider;
    }

    [HttpPost("executeProxied")]
    public async Task<ActionResult> ExecuteProxiedAsync([FromServices] IMediator mediator,
                                                        [FromServices] IFluentParameters fluentParameters,
                                                        ProxyReq req) {
        var model = _jsonProvider.DeserializeObject(req.RequestBody, req.RequestType);

        foreach (var (name, value) in req.ParameterData.OrEmpty()) {
            fluentParameters.Add(name, value);
        }
                    
        await mediator.SendAsync(req.CommandType, typeof(None), model);
        
        return Ok();
    }
}