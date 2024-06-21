using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Controllers;

[ApiDocument(CrowdfundingConstants.ApiName)]
public partial class CrowdfundingController : ApiController {
    private readonly IMediator _mediator;

    public CrowdfundingController(IMediator mediator) {
        _mediator = mediator;
    }
}