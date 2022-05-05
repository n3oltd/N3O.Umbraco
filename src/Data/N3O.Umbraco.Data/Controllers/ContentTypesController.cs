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

        [HttpGet("{contentId:guid}/allowed")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ContentTypeSummary>>> GetAllowedContentTypes() {
            try {
                var res = await _mediator.SendAsync<GetAllowedContentTypesQuery, None, IEnumerable<ContentTypeSummary>>(None.Empty);

                return Ok(res);
            } catch (ResourceNotFoundException ex) {
                return NotFound(ex);
            }
        }
    }
}