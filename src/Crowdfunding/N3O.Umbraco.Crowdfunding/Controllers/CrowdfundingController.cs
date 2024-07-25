using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using System;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Web.Common.Filters;

namespace N3O.Umbraco.Crowdfunding.Controllers;

[ApiDocument(CrowdfundingConstants.ApiName)]
[UmbracoMemberAuthorize]
public partial class CrowdfundingController : ApiController {
    private readonly Lazy<IMediator> _mediator;
    private readonly Lazy<ILookups> _lookups;
    private readonly Lazy<IUmbracoMapper> _mapper;

    public CrowdfundingController(Lazy<IMediator> mediator, Lazy<ILookups> lookups, Lazy<IUmbracoMapper> mapper) {
        _mediator = mediator;
        _lookups = lookups;
        _mapper = mapper;
    }
}