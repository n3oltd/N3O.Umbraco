using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Payments.Worldline.Controllers;

[ApiDocument(WorldlineConstants.ApiName)]
public class WorldlineController : ApiController {
    private readonly ILogger<WorldlineController> _logger;
    private readonly IMediator _mediator;

    public WorldlineController(ILogger<WorldlineController> logger, IMediator mediator) {
        _logger = logger;
        _mediator = mediator;
    }
}
