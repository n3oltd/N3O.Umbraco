using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Plugins.Extensions;
using N3O.Umbraco.Plugins.Models;
using N3O.Umbraco.Uploader.Models;
using N3O.Umbraco.Validation;
using N3O.Umbraco.Validation.Hosting.Controllers;
using NodaTime;
using System;
using System.IO;
using System.Threading.Tasks;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.Uploader.Controllers;

[ApiDocument(UploaderConstants.ApiName)]
public class UploaderController : ValidatingPluginController {
    private readonly IClock _clock;
    private readonly MediaFileManager _mediaFileManager;

    public UploaderController(IClock clock,
                              MediaFileManager mediaFileManager,
                              IValidation validation,
                              Lazy<IValidationHandler> validationHandler) : base(validation, validationHandler) {
        _clock = clock;
        _mediaFileManager = mediaFileManager;
    }

    [HttpGet("media/{mediaId}")]
    public ActionResult<FileMedia> GetMediaById(string mediaId) {
        var file = _mediaFileManager.GetSourceFile(mediaId);

        if (file == null) {
            return NotFound();
        }

        using (var stream = _mediaFileManager.FileSystem.OpenFile(file)) {
            return Ok(GetResponse(file, stream.Length));
        }
    }
    
    [HttpPost("upload")]
    public async Task<ActionResult<FileMedia>> Upload([FromForm] FileUploadReq req) {
        await ValidateAsync(req);
        
        var now = _clock.GetCurrentInstant();
    
        using (var uploadedFile = await GetUploadedFileAsync(req)) {
            if (uploadedFile == null) {
                return BadRequest();
            }
            
            var storagePath = uploadedFile.Filename.GetStoragePath(now);

            _mediaFileManager.FileSystem.AddFile(storagePath, uploadedFile.Stream, false);

            return Ok(GetResponse(storagePath, uploadedFile.Bytes));
        }
    }

    public FileMedia GetResponse(string storagePath, long filesizeBytes) {
        var mediaId = Path.GetDirectoryName(storagePath);
        var filename = Path.GetFileName(storagePath);
        var extension = Path.GetExtension(filename).ToLowerInvariant();

        var filesizeMb = filesizeBytes / (1024d * 1024d);

        return new FileMedia {
            UrlPath = "/media/" + mediaId + "/" + filename,
            MediaId = mediaId,
            Filename = filename,
            Extension = extension,
            SizeMb = Math.Round(filesizeMb, 2)
        };
    }
}
