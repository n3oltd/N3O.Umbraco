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
using N3O.Umbraco.Parameters;
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
    private readonly IFluentParameters _fluentParameters;
    private readonly IContentService _contentService;
    private readonly IContentCache _contentCache;
    private readonly ISlugHelper _slugHelper;

    public PlatformsBackOfficeController(IMediator mediator,
                                         IContentTypeService contentTypeService,
                                         IFluentParameters fluentParameters,
                                         IContentService contentService,
                                         IContentCache contentCache,
                                         ISlugHelper slugHelper) {
        _mediator = mediator;
        _contentTypeService = contentTypeService;
        _fluentParameters = fluentParameters;
        _contentService = contentService;
        _contentCache = contentCache;
        _slugHelper = slugHelper;
    }

    // In v17 UmbDocumentDetailModel.documentType has {unique, icon, collection} but no alias field,
    // so the frontend passes the document type GUID; alias is resolved server-side.
    [HttpPost("previewHtml/{documentTypeKey:guid}")]
    public async Task<ActionResult<PreviewHtmlRes>> GetPreviewHtml([FromRoute] Guid documentTypeKey,
                                                                   Dictionary<string, object> req) {
        var contentType = _contentTypeService.Get(documentTypeKey);

        if (contentType == null) {
            return NotFound();
        }

        _fluentParameters.Add("contentTypeAlias", contentType.Alias);

        var res = await _mediator.SendAsync<GetPreviewHtmlQuery, Dictionary<string, object>, PreviewHtmlRes>(req);

        return Ok(res);
    }

    // Returns the staging and production public URLs for a campaign or offering document. Also acts
    // as the visibility endpoint for the N3O.WorkspaceView.PlatformsUrls workspace-view condition
    // (the condition reads the `permitted` field; extra fields are ignored).
    [HttpGet("contentUrls/{contentId:guid}")]
    public ActionResult<ContentUrlsRes> GetContentUrls([FromRoute] Guid contentId) {
        var content = _contentService.GetById(contentId);

        if (content == null) {
            return Ok(new ContentUrlsRes { Permitted = false });
        }

        var contentType = _contentTypeService.Get(content.ContentTypeId);

        if (contentType == null) {
            return Ok(new ContentUrlsRes { Permitted = false });
        }

        var compositionAliases = contentType.CompositionAliases();
        var isCampaign = compositionAliases.ContainsAny([PlatformsConstants.Campaigns.CompositionAlias], true);
        var isOffering = compositionAliases.ContainsAny([PlatformsConstants.Offerings.CompositionAlias], true);

        if (!isCampaign && !isOffering) {
            return Ok(new ContentUrlsRes { Permitted = false });
        }

        var urlSettings = _contentCache.Single<UrlSettingsContent>();

        if (urlSettings == null) {
            return Ok(new ContentUrlsRes { Permitted = true });
        }

        string stagingUrl = null;
        string productionUrl = null;

        if (isCampaign) {
            var path = _contentCache.GetCampaignPath(_slugHelper, content.Name);

            if (path.HasValue()) {
                if (urlSettings.StagingBaseUrl.HasValue()) {
                    stagingUrl = new Url(urlSettings.StagingBaseUrl).AppendPathSegment(path);
                }

                if (urlSettings.ProductionBaseUrl.HasValue()) {
                    productionUrl = new Url(urlSettings.ProductionBaseUrl).AppendPathSegment(path);
                }
            }
        } else {
            var parent = _contentService.GetById(content.ParentId);

            if (parent != null) {
                var path = _contentCache.GetOfferingPath(_slugHelper, parent.Name, content.Name);

                if (path.HasValue()) {
                    if (urlSettings.StagingBaseUrl.HasValue()) {
                        stagingUrl = new Url(urlSettings.StagingBaseUrl).AppendPathSegment(path);
                    }

                    if (urlSettings.ProductionBaseUrl.HasValue()) {
                        productionUrl = new Url(urlSettings.ProductionBaseUrl).AppendPathSegment(path);
                    }
                }
            }
        }

        return Ok(new ContentUrlsRes { Permitted = true, StagingUrl = stagingUrl, ProductionUrl = productionUrl });
    }
}
