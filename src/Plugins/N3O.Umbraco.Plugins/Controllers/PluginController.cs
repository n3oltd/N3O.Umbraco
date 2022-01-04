using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Hosting;
using System.Net.Mime;

namespace N3O.Umbraco.Plugins.Controllers {
    [ApiController]
    [Route("/umbraco/backoffice/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public abstract partial class PluginController : ApiController { }
}
