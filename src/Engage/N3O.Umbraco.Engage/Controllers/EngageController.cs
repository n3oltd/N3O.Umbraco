using N3O.Umbraco.Attributes;
using N3O.Umbraco.Engage.Models;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Engage.Controllers;

[ApiDocument(EngageConstants.ApiName)]
public class EngageController : LookupsController<EngageLookupsRes> {
    private readonly IMediator _mediator;

    public EngageController(ILookups lookups, IUmbracoMapper mapper, IMediator mediator)
        : base(lookups, mapper) {
        _mediator = mediator;
    }
}