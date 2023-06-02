using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Storage.Models;
using NodaTime;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace N3O.Umbraco.Storage.Controllers;

[ApiDocument(StorageConstants.ApiName)]
public class StorageController : ApiController {
    private readonly ILogger<StorageController> _logger;
    private readonly IClock _clock;
    private readonly IVolume _volume;

    public StorageController(ILogger<StorageController> logger, IClock clock, IVolume volume) {
        _logger = logger;
        _clock = clock;
        _volume = volume;
    }
    
    [HttpPost("download/{folderName}/{filename}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Download(string folderName, string filename) {
        try {
            var storageFolder = await _volume.GetStorageFolderAsync(folderName);
            var blob = await storageFolder.GetFileAsync(filename);

            return File(blob.Stream, blob.ContentType, blob.Filename);
        } catch {
            return NotFound();
        }
    }
    
    [HttpPost("tempUpload")]
    [RequestSizeLimit(500_000_000)]
    public async Task<ActionResult<StorageToken>> TempUpload([FromForm] UploadReq req) {
        var folderPath = Path.Join(StorageConstants.StorageFolders.Temp,
                                         $"_{_clock.GetCurrentInstant().ToUnixTimeTicks()}");
        
        return await Upload(folderPath, req);
    }

    [HttpPost("upload/{folderPath}")]
    public async Task<ActionResult<StorageToken>> Upload(string folderPath, [FromForm] UploadReq req) {
        try {
            using (var reqStream = req.File.OpenReadStream()) {
                var filename = Sanitise(req.File.FileName);
                var storageFolder = await _volume.GetStorageFolderAsync(folderPath);
                await storageFolder.AddFileAsync(filename, reqStream);

                var blob = await storageFolder.GetFileAsync(filename);

                using (blob.Stream) {
                    return Ok(StorageToken.FromBlob(blob));
                }
            }
        } catch (Exception ex) {
            _logger.LogError(ex, "File upload failed");
            
            return BadRequest();
        }
    }

    private string Sanitise(string filename) {
        return Regex.Replace(filename,
                             @"[^a-z0-9\-_\.]",
                             "",
                             RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    }
}
