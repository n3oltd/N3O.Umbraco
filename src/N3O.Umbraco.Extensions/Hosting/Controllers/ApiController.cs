using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Umbraco.Cms.Web.Common.Controllers;

namespace N3O.Umbraco.Hosting {
    [ApiController]
    [Route("/umbraco/api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ApiController : UmbracoApiController { }
}