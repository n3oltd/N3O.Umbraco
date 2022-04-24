using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Plugins.Controllers;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data.Controllers {
    [ApiDocument(DataConstants.ApiNames.Import)]
    public class ImportController : PluginController {
        private readonly ILogger<ImportController> _logger;
        private readonly IMediator _mediator;

        public ImportController(ILogger<ImportController> logger, IMediator mediator) {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("template/{contentId:guid}")]
        public async Task<ActionResult> GetTemplate() {
            var res = await _mediator.SendAsync<GetImportTemplateQuery, None, ImportTemplate>(None.Empty);

            return File(res.Contents, DataConstants.ContentTypes.Csv, res.Filename);
        }

        [HttpPost("queue/{contentId:guid}")]
        public async Task<ActionResult> Queue([FromForm] QueueImportsReq req) {
            try {
                await _mediator.SendAsync<QueueImportsCommand, QueueImportsReq>(req);

                return Ok();
            } catch (Exception ex) {
                _logger.LogError(ex, "Import failed");
                
                return UnprocessableEntity();
            }
        }
    }
}