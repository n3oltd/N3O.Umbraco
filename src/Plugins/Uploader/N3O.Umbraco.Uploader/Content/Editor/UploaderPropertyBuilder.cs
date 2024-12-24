using Humanizer.Bytes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Uploader.DataTypes;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Plugins.Extensions;
using Newtonsoft.Json;
using System;
using System.IO;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Uploader.Content;

public class UploaderPropertyBuilder : PropertyBuilder {
    private readonly MediaFileManager _mediaFileManager;
    private readonly ILocalClock _clock;
    private string _urlPath;
    private string _filename;
    private string _altText;
    private ByteSize _fileSize;

    public UploaderPropertyBuilder(IContentTypeService contentTypeService,
                                   MediaFileManager mediaFileManager,
                                   ILocalClock clock)
        : base(contentTypeService) {
        _mediaFileManager = mediaFileManager;
        _clock = clock;
    }
    
    public UploaderPropertyBuilder SetAltText(string altText) {
        _altText = altText;

        return this;
    }

    public UploaderPropertyBuilder SetFile(string mediaId) {
        _urlPath = _mediaFileManager.GetSourceFile(mediaId);

        if (_urlPath == null) {
            throw new Exception($"No media found with ID {mediaId}");
        }

        return this;
    }

    public UploaderPropertyBuilder SetFile(Blob blob) {
        return SetFile(blob.Stream, blob.Filename);
    }
    
    public UploaderPropertyBuilder SetFile(byte[] bytes, string filename) {
        using (var stream = new MemoryStream(bytes)) {
            SetFile(stream, filename);

            return this;
        }
    }

    public UploaderPropertyBuilder SetFile(Stream stream, string filename) {
        var instant = _clock.GetCurrentInstant();

        stream.Rewind();

        if (!_mediaFileManager.FileSystem.FileExists(filename.GetStoragePath(instant))) {
            _mediaFileManager.FileSystem.AddFile(filename.GetStoragePath(instant), stream, false);
        }

        _urlPath = filename.GetMediaUrlPath(instant);
        _filename = filename;
        _fileSize = ByteSize.FromBytes(stream.Length);

        return this;
    }

    public override (object, IPropertyType) Build(string propertyAlias, string contentTypeAlias) {
        Validate();

        var uploaderSource = new UploaderSource();
        uploaderSource.AltText = _altText;
        uploaderSource.Extension = Path.GetExtension(_filename).ToLowerInvariant();
        uploaderSource.Filename = _filename;
        uploaderSource.SizeMb = Math.Round(_fileSize.Megabytes, 2);
        uploaderSource.UrlPath = _urlPath;

        Value = JsonConvert.SerializeObject(uploaderSource);

        return (Value, GetPropertyType(propertyAlias, contentTypeAlias));
    }

    private void Validate() {
        if (!_urlPath.HasValue()) {
            throw new Exception("File must be specified");
        }
    }
}
