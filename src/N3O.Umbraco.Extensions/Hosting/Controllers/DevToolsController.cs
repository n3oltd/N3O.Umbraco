using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Hosting;

[ApiDocument("DevTools")]
public class DevToolsController : ApiController {
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DevToolsController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment) {
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet("core/configuration")]
    public ActionResult<string> GetConfiguration() {
        var root = (IConfigurationRoot) _configuration;
        var debugView = root.GetDebugView();

        return Result(debugView);
    }

    private ActionResult<T> Result<T>(T result) {
        var isDevelopment = _webHostEnvironment.IsDevelopment();
            
        if (!isDevelopment) {
            return Unauthorized();
        }

        return Ok(result);
    }
}
