using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<ExportsController> _logger;
    private readonly IMediator _mediator;
    private readonly Lazy<ILookups> _lookups;
    private readonly Lazy<IUmbracoMapper> _mapper;

    public ExportsController(ILogger<ExportsController> logger,
                             IMediator mediator,
                             Lazy<ILookups> lookups,
                             Lazy<IUmbracoMapper> mapper) {
        _logger = logger;
        _mediator = mediator;
        _lookups = lookups;
        _mapper = mapper;
    }
    
    [HttpGet("lookups/contentMetadata")]
    public async Task<ActionResult<IEnumerable<ContentMetadataRes>>> GetLookupContentMetadata() {
        var listLookups = new ListCustomLookups<ContentMetadata, ContentMetadataRes>(_lookups.Value, _mapper.Value);
        var res = await listLookups.RunAsync();

        return Ok(res);
    }

    [HttpGet("exportableProperties/{contentType}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ExportableProperty>>> GetExportableProperties() {
        try {
            var res = await _mediator.SendAsync<GetExportablePropertiesQuery, None, ExportableProperties>(None.Empty);

            return Ok(res.Properties);
        } catch (ResourceNotFoundException ex) {
            return NotFound(ex);
        }
    }

    [HttpPost("export/{contentId:guid}/{contentType}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CreateExport(ExportReq req) {
        try {
            var res = await _mediator.SendAsync<CreateExportCommand, ExportReq, ExportFile>(req);

            return File(res.Contents, res.ContentType, res.Filename);
        } catch (ResourceNotFoundException ex) {
            return NotFound(ex);
        } catch (Exception ex) {
            _logger.LogError(ex, "Export failed");
            
            return UnprocessableEntity("Error generating export, please contact support");
        }
    }
}
