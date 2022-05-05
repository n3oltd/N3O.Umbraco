using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Plugins.Controllers;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data.Controllers {
    [ApiDocument(DataConstants.ApiNames.Exports)]
    public class ExportsController : PluginController {
        private readonly ILogger<ExportsController> _logger;
        private readonly IMediator _mediator;

        public ExportsController(ILogger<ExportsController> logger, IMediator mediator) {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("exportableProperties/{contentId:guid}/{contentType}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetExportableProperties() {
            try {
                var res = await _mediator.SendAsync<GetExportablePropertiesQuery, None, ExportableProperties>(None.Empty);

                return Ok(res);
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
                
                return UnprocessableEntity();
            }
        }
    }
}