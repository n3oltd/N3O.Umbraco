using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Plugins.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Data.Controllers {
    [ApiDocument(DataConstants.ApiNames.Import)]
    public class ImportController : PluginController {
        private readonly ILogger<ImportController> _logger;
        private readonly ILookups _lookups;
        private readonly IUmbracoMapper _mapper;
        private readonly IMediator _mediator;

        public ImportController(ILogger<ImportController> logger, ILookups lookups, IUmbracoMapper mapper, IMediator mediator) {
            _logger = logger;
            _lookups = lookups;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet("template/{contentId:guid}")]
        public async Task<ActionResult> GetTemplate() {
            var res = await _mediator.SendAsync<GetImportTemplateQuery, None, ImportTemplate>(None.Empty);

            return File(res.Contents, DataConstants.ContentTypes.Csv, res.Filename);
        }

        // TODO add upload endpoint which receives file and return a storage token. 
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
        
        [HttpGet("lookups/datePattern")]
        public async Task<ActionResult<IEnumerable<NamedLookupRes>>> GetLookupAllocationTypes() {
            var listLookups = new ListLookups<DatePattern>(_lookups, _mapper);
            var res = await listLookups.RunAsync();

            return Ok(res);
        }
    }
}