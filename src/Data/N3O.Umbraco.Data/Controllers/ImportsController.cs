using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    private readonly Lazy<ILookups> _lookups;
    private readonly Lazy<IUmbracoMapper> _mapper;
    private readonly Lazy<IMediator> _mediator;
    
    public ImportsController(Lazy<ILookups> lookups,
                             Lazy<IUmbracoMapper> mapper,
                             Lazy<IMediator> mediator) {
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
        }
    }
    
    [HttpGet("lookups/datePatterns")]
    public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupDatePatterns() {
        var listLookups = new ListLookups<DatePattern>(_lookups.Value, _mapper.Value);
        var res = await listLookups.RunAsync();

        return Ok(res);
    }

    [HttpPost("template/{contentType}")]
    public async Task<FileContentResult> GetTemplate(ImportTemplateReq req) {
        var res = await _mediator.Value.SendAsync<GetImportTemplateQuery, ImportTemplateReq, ImportTemplate>(req);

        return File(res.Contents, DataConstants.ContentTypes.Csv, res.Filename);
    }
    
    [HttpGet("importableProperties/{contentType}")]
    public async Task<ActionResult<IEnumerable<DataProperty>>> GetImportableProperties() {
        try {
            var res = await _mediator.Value.SendAsync<GetImportablePropertiesQuery, None, DataProperties>(None.Empty);

            return Ok(res.Properties);
        } catch (ResourceNotFoundException ex) {
            return NotFound(ex);
        }
    }
    
    [HttpPost("queue/{containerId:guid}/{contentType}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QueueImportsRes>> Queue(QueueImportsReq req) {
        try {
            var res = await _mediator.Value.SendAsync<QueueImportsCommand, QueueImportsReq, QueueImportsRes>(req);

            return Ok(res);
        } catch (ProcessingException ex) {
            return UnprocessableEntity(ex.Errors);
        }
    }
    
    [HttpPut("requeueFailed")]
    public async Task<ActionResult> RequeueFailed() {
        await _mediator.Value.SendAsync<RequeueFailedImportsCommand, None, None>(None.Empty);

        return Ok();
    }
}
