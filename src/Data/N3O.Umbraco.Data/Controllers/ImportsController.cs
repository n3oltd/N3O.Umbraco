using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Exceptions;
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

[ApiDocument(DataConstants.ApiNames.Imports)]
public class ImportsController : PluginController {
    private readonly ILogger<ImportsController> _logger;
    private readonly Lazy<ILookups> _lookups;
    private readonly Lazy<IUmbracoMapper> _mapper;
    private readonly Lazy<IMediator> _mediator;
    
    public ImportsController(ILogger<ImportsController> logger,
                             Lazy<ILookups> lookups,
                             Lazy<IUmbracoMapper> mapper,
                             Lazy<IMediator> mediator) {
        _logger = logger;
        _lookups = lookups;
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpPost("queued/{referenceId}/files")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AddFileToImport(AddFileToImportReq req) {
        try {
            var res = await _mediator.Value.SendAsync<AddFileToImportCommand, AddFileToImportReq, None>(req);

            return Ok(res);
        } catch (ResourceNotFoundException ex) {
            return NotFound(ex);
        } catch (Exception ex) {
            _logger.LogError(ex, "Adding file to import failed");
            
            return UnprocessableEntity("Error uploading file, please contact support");
        }
    }
    
    [HttpGet("lookups/datePatterns")]
    public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupDatePatterns() {
        var listLookups = new ListLookups<DatePattern>(_lookups.Value, _mapper.Value);
        var res = await listLookups.RunAsync();

        return Ok(res);
    }

    [HttpGet("template/{contentType}")]
    public async Task<FileContentResult> GetTemplate() {
        var res = await _mediator.Value.SendAsync<GetImportTemplateQuery, None, ImportTemplate>(None.Empty);

        return File(res.Contents, DataConstants.ContentTypes.Csv, res.Filename);
    }
    
    [HttpPost("queue/{containerId:guid}/{contentType}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QueueImportsRes>> Queue(QueueImportsReq req) {
        try {
            var res = await _mediator.Value.SendAsync<QueueImportsCommand, QueueImportsReq, QueueImportsRes>(req);

            return Ok(res);
        } catch (ProcessingException ex) {
            return UnprocessableEntity(ex.Errors);
        } catch (Exception ex) {
            _logger.LogError(ex, "Import failed");

            return UnprocessableEntity("Error queuing records for import, please contact support");
        }
    }
    
    [HttpPut("requeueFailed")]
    public async Task<ActionResult> RequeueFailed() {
        await _mediator.Value.SendAsync<RequeueFailedImportsCommand, None, None>(None.Empty);

        return Ok();
    }
}
