using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using System;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Controllers;

[ApiDocument(PlatformsConstants.PreviewApiName)]
public class PlatformsPreviewController : BackofficeAuthorizedApiController {
    private static readonly string[] CompositionAliases = [PlatformsConstants.Offerings.CompositionAlias];

    private readonly IContentService _contentService;
    private readonly IContentTypeService _contentTypeService;

    public PlatformsPreviewController(IContentService contentService, IContentTypeService contentTypeService) {
        _contentService = contentService;
        _contentTypeService = contentTypeService;
    }

    [HttpGet("visibility/{contentId:guid}")]
    public ActionResult<WorkspaceVisibilityRes> GetVisibility([FromRoute] Guid contentId) {
        return Ok(new WorkspaceVisibilityRes { Visible = IsPermitted(contentId) });
    }

    private bool IsPermitted(Guid contentId) {
        var content = _contentService.GetById(contentId);

        if (content == null) {
            return false;
        }

        var contentType = _contentTypeService.Get(content.ContentTypeId);

        if (contentType == null) {
            return false;
        }

        return contentType.CompositionAliases().ContainsAny(CompositionAliases, true);
    }
}
