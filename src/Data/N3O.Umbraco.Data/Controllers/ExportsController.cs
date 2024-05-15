using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Plugins.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Data.Controllers;

[ApiDocument(DataConstants.ApiNames.Exports)]
public class ExportsController : PluginController {
    private readonly IMediator _mediator;
    private readonly Lazy<ILookups> _lookups;
    private readonly Lazy<IUmbracoMapper> _mapper;

    public ExportsController(IMediator mediator,
                             Lazy<ILookups> lookups,
                             Lazy<IUmbracoMapper> mapper) {
        _mediator = mediator;
        _lookups = lookups;
        _mapper = mapper;
    }

    [HttpPost("export/{containerId:guid}/{contentType}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExportProgressRes>> CreateExport(ExportReq req) {
        try {
            var res = await _mediator.SendAsync<CreateExportCommand, ExportReq, ExportProgressRes>(req);

            return Ok(res);
        } catch (ResourceNotFoundException ex) {
            return NotFound(ex);
        }
    }

    [HttpGet("exportableProperties/{contentType}")]
    public async Task<ActionResult<IEnumerable<ExportableProperty>>> GetExportableProperties() {
        try {
            var res = await _mediator.SendAsync<GetExportablePropertiesQuery, None, ExportableProperties>(None.Empty);

            return Ok(res.Properties);
        } catch (ResourceNotFoundException ex) {
            return NotFound(ex);
        }
    }

    [HttpGet("export/{exportId:entityId}/file")]
    public async Task<FileResult> GetExportFile() {
        var res = await _mediator.SendAsync<GetExportFileQuery, None, ExportFile>(None.Empty);

        return File(res.Contents, res.ContentType, res.Filename);
    }

    [HttpGet("export/{exportId:entityId}/progress")]
    public async Task<ActionResult<ExportProgressRes>> GetExportProgress() {
        try {
            var res = await _mediator.SendAsync<GetExportProgressQuery, None, ExportProgressRes>(None.Empty);

            return Ok(res);
        } catch (ResourceNotFoundException ex) {
            return NotFound(ex);
        }
    }
    
    [HttpGet("lookups/contentMetadata")]
    public async Task<ActionResult<IEnumerable<ContentMetadataRes>>> GetLookupContentMetadata() {
        var listLookups = new ListCustomLookups<ContentMetadata, ContentMetadataRes>(_lookups.Value, _mapper.Value);
        var res = await listLookups.RunAsync();

        return Ok(res);
    }
}
