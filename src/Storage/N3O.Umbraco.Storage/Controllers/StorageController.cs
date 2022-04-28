using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Storage.Models;
using N3O.Umbraco.Storage.Services;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Storage.Controllers {
    [ApiDocument(StorageConstants.ApiName)]
    public class StorageController : ApiController {
        private readonly ILogger<StorageController> _logger;
        private readonly ITempStorage _tempStorage;

        public StorageController(ILogger<StorageController> logger, ITempStorage tempStorage) {
            _logger = logger;
            _tempStorage = tempStorage;
        }
        
        [HttpPost("download/{filename}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DownloadAsync(string filename) {
            try {
                var blob = await _tempStorage.GetFileAsync(filename);

                return File(blob.Stream, blob.ContentType, blob.Filename);
            } catch {
                return NotFound();
            }
        }
        
        [HttpPost("upload")]
        public async Task<ActionResult<StorageToken>> UploadAsync([FromForm] UploadReq req) {
            try {
                using (var reqStream = req.File.OpenReadStream()) {
                    var blob = await _tempStorage.AddFileAsync(req.File.Name, reqStream);
                    
                    return Ok(StorageToken.FromBlob(blob));
                }
            } catch (Exception ex) {
                _logger.LogError(ex, "File upload failed");
                
                return BadRequest();
            }
        }
    }
}