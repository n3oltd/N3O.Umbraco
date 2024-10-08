﻿using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Giving.Cart.Context;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Json;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Filters;

namespace N3O.Umbraco.Crowdfunding.Controllers;

[ApiDocument(CrowdfundingConstants.ApiName)]
[UmbracoMemberAuthorize]
public partial class CrowdfundingController : ApiController {
    private readonly Lazy<IMediator> _mediator;
    private readonly Lazy<IContentService> _contentService;
    private readonly Lazy<FundraiserAccessControl> _fundraiserAccessControl;
    private readonly Lazy<ILookups> _lookups;
    private readonly Lazy<IUmbracoMapper> _mapper;
    private readonly Lazy<IJsonProvider> _jsonProvider;
    private readonly Lazy<CartCookie> _cartCookie;
    private readonly Lazy<IContentLocator> _contentLocator;

    public CrowdfundingController(Lazy<IMediator> mediator,
                                  Lazy<IContentService> contentService,
                                  Lazy<FundraiserAccessControl> fundraiserAccessControl,
                                  Lazy<ILookups> lookups,
                                  Lazy<IUmbracoMapper> mapper,
                                  Lazy<IJsonProvider> jsonProvider,
                                  Lazy<CartCookie> cartCookie,
                                  Lazy<IContentLocator> contentLocator) {
        _mediator = mediator;
        _contentService = contentService;
        _fundraiserAccessControl = fundraiserAccessControl;
        _lookups = lookups;
        _mapper = mapper;
        _jsonProvider = jsonProvider;
        _cartCookie = cartCookie;
        _contentLocator = contentLocator;
    }
    
    [HttpPost("suggestSlug")]
    public async Task<ActionResult<string>> SuggestSlug([FromQuery] string name) {
        var res = await _mediator.Value.SendAsync<SuggestSlugQuery, string, string>(name);

        return Ok(res);
    }

    private async Task EnforceFundraiserAccessControlsAsync(Guid contentId) {
        var content = _contentService.Value.GetById(contentId);

        var canEdit = await _fundraiserAccessControl.Value.CanEditAsync(content);

        if (!canEdit) {
            throw new UnauthorizedAccessException();
        }
    }
}