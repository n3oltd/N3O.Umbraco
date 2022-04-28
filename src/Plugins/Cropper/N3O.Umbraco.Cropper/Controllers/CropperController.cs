using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Plugins.Controllers;
using N3O.Umbraco.Plugins.Extensions;
using N3O.Umbraco.Plugins.Models;
using NodaTime;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.Cropper.Controllers {
    [ApiDocument(CropperConstants.ApiName)]
    public class CropperController : PluginController {
        private readonly IClock _clock;
        private readonly MediaFileManager _mediaFileManager;

        public CropperController(IClock clock, MediaFileManager mediaFileManager) {
            _clock = clock;
            _mediaFileManager = mediaFileManager;
        }

        [HttpGet("media/{mediaId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ImageMedia> GetMediaById(string mediaId) {
            var file = _mediaFileManager.FileSystem
                                        .GetFiles(mediaId, "*.*")
                                        .OrderBy(x => _mediaFileManager.FileSystem.GetLastModified(x))
                                        .FirstOrDefault();

            if (file == null) {
                return NotFound();
            }

            using (var stream = _mediaFileManager.FileSystem.OpenFile(file)) {
                var metadata = stream.GetImageMetadata();

                return Ok(GetResponse(file, metadata.Height, metadata.Width));
            }
        }

        [HttpPost("upload")]
        public async Task<ActionResult<ImageMedia>> Upload([FromForm] ImageUploadReq req) {
            var now = _clock.GetCurrentInstant();
        
            using (var uploadedImage = await GetUploadedImageAsync(req)) {
                if (uploadedImage == null) {
                    return BadRequest();
                }

                // TODO move this to extension method and later use in here and cropperBuilder.
                var storagePath = Path.Combine(now.ToString("yyMMddHHmmss", CultureInfo.InvariantCulture),
                                               uploadedImage.Filename);

                _mediaFileManager.FileSystem.AddFile(storagePath, uploadedImage.Stream, false);

                return Ok(GetResponse(storagePath, uploadedImage.Metadata.Height, uploadedImage.Metadata.Width));
            }
        }

        private ImageMedia GetResponse(string storagePath, int height, int width) {
            var mediaId = Path.GetDirectoryName(storagePath);
            var filename = Path.GetFileName(storagePath);

            return new ImageMedia {
                UrlPath = "/media/" + mediaId + "/" + filename,
                MediaId = mediaId,
                Filename = filename,
                Height = height,
                Width = width
            };
        }
    }
}