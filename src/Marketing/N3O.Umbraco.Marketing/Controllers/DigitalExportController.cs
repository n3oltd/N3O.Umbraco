using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Marketing.Content;
using N3O.Umbraco.Marketing.Models;
using N3O.Umbraco.Marketing.Services;
using NodaTime.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Marketing.Controllers;

[ApiDocument(MarketingConstants.ApiName)]
public class DigitalExportController : ApiController {
    private readonly IContentCache _contentCache;
    private readonly IDigitalExport _digitalExport;

    public DigitalExportController(IContentCache contentCache, IDigitalExport digitalExport) {
        _contentCache = contentCache;
        _digitalExport = digitalExport;
    }

    [HttpGet("v1/daily")]
    public async Task<ActionResult<DailyRes>> GetDaily(string siteId,
                                                       string from,
                                                       string to,
                                                       CancellationToken cancellationToken) {
        if (!IsAuthorized()) {
            return Unauthorized();
        }

        var fromResult = LocalDatePattern.Iso.Parse(from ?? "");
        var toResult = LocalDatePattern.Iso.Parse(to ?? "");

        if (!fromResult.Success || !toResult.Success) {
            return BadRequest("from and to must be dates in the format yyyy-MM-dd");
        }

        if (fromResult.Value > toResult.Value) {
            return BadRequest("from must not be later than to");
        }

        var res = await _digitalExport.GetDailyAsync(siteId, fromResult.Value, toResult.Value, cancellationToken);

        if (res == null) {
            return NotFound();
        }

        if (!res.Goals.Any() &&
            !res.Traffic.Any() &&
            !await _digitalExport.HasRecordedTrafficAsync(siteId, cancellationToken)) {
            return Conflict("Umbraco Engage has recorded no pageviews for this site's configured host");
        }

        return Ok(res);
    }

    [HttpGet("v1/sites")]
    public ActionResult<SitesRes> GetSites() {
        if (!IsAuthorized()) {
            return Unauthorized();
        }

        var res = new SitesRes();
        res.Sites = _digitalExport.GetSites();

        return Ok(res);
    }

    [HttpGet("v1/sites/{siteId}")]
    public ActionResult<SiteRes> GetSite(string siteId) {
        if (!IsAuthorized()) {
            return Unauthorized();
        }

        var res = _digitalExport.GetSite(siteId);

        if (res == null) {
            return NotFound();
        }

        return Ok(res);
    }

    private bool IsAuthorized() {
        var settings = _contentCache.Single<DigitalExportSettingsContent>();
        var exportKey = settings?.ExportKey;

        if (!exportKey.HasValue()) {
            return false;
        }

        Request.Headers.TryGetValue(MarketingConstants.HttpHeaders.ExportKey, out var suppliedKey);

        // Compared in fixed time because the export key is a long-lived secret an attacker can probe
        return CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(exportKey),
                                                       Encoding.UTF8.GetBytes(suppliedKey.ToString()));
    }
}
