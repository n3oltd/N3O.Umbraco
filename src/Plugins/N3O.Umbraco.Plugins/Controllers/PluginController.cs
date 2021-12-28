using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Umbraco.Cms.Web.BackOffice.Controllers;

namespace N3O.Umbraco.Plugins.Controllers;

[ApiController]
[Route("/umbraco/backoffice/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public abstract partial class PluginController : UmbracoAuthorizedApiController { }
