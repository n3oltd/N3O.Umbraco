using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data.Controllers {
    // TODO Add authentication to this controller
    [ApiDocument(DataConstants.ApiNames.ContentTypes)]
    public class ContentTypesController : ApiController {
        private readonly IMediator _mediator;

        public ContentTypesController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpPost("{contentType}")]
        public async Task<ActionResult<ContentTypeRes>> GetContentTypeByAlias() {
            try {
                var res = await _mediator.SendAsync<GetContentTypeByAliasQuery, None, ContentTypeRes>(None.Empty);

                return Ok(res);
            } catch (ResourceNotFoundException ex) {
                return NotFound(ex);
            }
        }

        [HttpGet("{contentId:guid}/relations")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ContentTypeSummary>>> GetRelationContentTypes([FromQuery] string type) {
            try {
                var res = await _mediator.SendAsync<GetRelationContentTypesQuery, string, IEnumerable<ContentTypeSummary>>(type);

                return Ok(res);
            } catch (ResourceNotFoundException ex) {
                return NotFound(ex);
            }
        }
    }
}