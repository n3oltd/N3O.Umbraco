using Flurl;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Cloud.Platforms.Queries;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Utilities;
using Slugify;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Controllers;

[ApiDocument(PlatformsConstants.BackOfficeApiName)]
public class PlatformsBackOfficeController : BackofficeAuthorizedApiController {
    private readonly IMediator _mediator;
    private readonly IContentTypeService _contentTypeService;
    private readonly IContentService _contentService;
    private readonly IContentCache _contentCache;
    private readonly ISlugHelper _slugHelper;

    public PlatformsBackOfficeController(IMediator mediator,
                                         IContentTypeService contentTypeService,
                                         IContentService contentService,
                                         IContentCache contentCache,
                                         ISlugHelper slugHelper) {
        _mediator = mediator;
        _contentTypeService = contentTypeService;
        _contentService = contentService;
        _contentCache = contentCache;
        _slugHelper = slugHelper;
    }

    [HttpPost("previewHtml/{contentId:guid}")]
    public async Task<ActionResult<PreviewHtmlRes>> GetPreviewHtml(Dictionary<string, object> req) {
        var res = await _mediator.SendAsync<GetPreviewHtmlQuery, Dictionary<string, object>, PreviewHtmlRes>(req);

        return Ok(res);
    }
    
    [HttpGet("contentUrls/{contentId:guid}")]
    public ActionResult<ContentUrlsRes> GetContentUrls([FromRoute] Guid contentId) {
        var content = _contentService.GetById(contentId);
        var urlSettings = _contentCache.Single<UrlSettingsContent>();

        var isCampaign = content != null && content.IsCampaign(_contentTypeService);
        var isOffering = content != null && content.IsOffering(_contentTypeService);

        if (content == null || urlSettings == null || (!isCampaign && !isOffering)) {
            return Ok(new ContentUrlsRes { Permitted = false });
        }

        string path;

        if (isCampaign) {
            path = _contentCache.GetCampaignPath(_slugHelper, content.Name);
        } else {
            var parent = _contentService.GetById(content.ParentId);

            path = parent == null ? null : _contentCache.GetOfferingPath(_slugHelper, parent.Name, content.Name);
        }

        var res = new ContentUrlsRes();
        res.Permitted = true;

        if (path.HasValue()) {
            if (urlSettings.StagingBaseUrl.HasValue()) {
                res.StagingUrl = new Url(urlSettings.StagingBaseUrl).AppendPathSegment(path);
            }

            if (urlSettings.ProductionBaseUrl.HasValue()) {
                res.ProductionUrl = new Url(urlSettings.ProductionBaseUrl).AppendPathSegment(path);
            }
        }

        return Ok(res);
    }
}
