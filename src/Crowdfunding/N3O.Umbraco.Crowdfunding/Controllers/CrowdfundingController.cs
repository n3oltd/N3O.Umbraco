using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using System;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Controllers;

[ApiDocument(CrowdfundingConstants.ApiName)]
public partial class CrowdfundingController : ApiController {
    private readonly IMediator _mediator;
    private readonly Lazy<ILookups> _lookups;
    private readonly Lazy<IUmbracoMapper> _mapper;

    public CrowdfundingController(IMediator mediator, Lazy<ILookups> lookups, Lazy<IUmbracoMapper> mapper) {
        _mediator = mediator;
        _lookups = lookups;
        _mapper = mapper;
    }
}