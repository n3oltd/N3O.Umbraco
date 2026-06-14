using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Cloud.Platforms.Queries;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;
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

    public PlatformsBackOfficeController(IMediator mediator,
                                         IContentTypeService contentTypeService,
                                         IFluentParameters fluentParameters) {
        _mediator = mediator;
        _contentTypeService = contentTypeService;
        _fluentParameters = fluentParameters;
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
}
