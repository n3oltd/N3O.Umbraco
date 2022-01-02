using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mime;
using Umbraco.Cms.Web.Common.Controllers;

namespace N3O.Umbraco.Hosting {
    [ApiController]
    [Route("/umbraco/api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ApiController : UmbracoApiController {
        public ApiController(ILogger logger) {
            Logger = logger;
        }
    
        protected ActionResult RequestFailed(Action<ILogger> logAction) {
            logAction(Logger);

            return UnprocessableEntity();
        }
    
    
        protected ActionResult<T> RequestFailed<T>(T value, Action<ILogger> logAction) {
            logAction(Logger);

            return UnprocessableEntity(value);
        }
    
        protected ILogger Logger { get; }
    }
}