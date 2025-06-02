using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;

namespace N3O.Umbraco.Telemetry.Controllers;

[ApiDocument(TelemetryConstants.ApiName)]
public class TelemetryController : ApiController {
    private readonly ITelemetryData _telemetryData;

    public TelemetryController(ITelemetryData telemetryData) {
        _telemetryData = telemetryData;
    }
    
    [HttpGet("extensions/version")]
    public ActionResult<string> GetExtensionsVersion() {
        var res = _telemetryData.GetExtensionsVersion();

        return Ok(res);
    }
}
