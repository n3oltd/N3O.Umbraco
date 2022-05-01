using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Plugins.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.Cropper.Content {
    public class CropperPropertyBuilder : PropertyBuilder {
        private readonly MediaFileManager _mediaFileManager;
        private readonly ILocalClock _clock;
        private readonly List<ICropBuilder> _cropBuilders = new();
        private string _src;
        private string _mediaId;
        private string _filename;
        private int? _width;
        private int? _height;
        private string _altText;

        public CropperPropertyBuilder(MediaFileManager mediaFileManager, ILocalClock clock) {
            _mediaFileManager = mediaFileManager;
            _clock = clock;
        }

        public ICropBuilder AddCrop() {
            if (_width == null || _height == null) {
                throw new Exception("Must set image before adding crops");
            }
            
            var cropBuilder = new CropBuilder(_width.Value, _height.Value);

            _cropBuilders.Add(cropBuilder);

            return cropBuilder;
        }

        public CropperPropertyBuilder AutoCrop(CropperConfiguration configuration) {
            foreach (var cropDefinition in configuration.CropDefinitions) {
                AddCrop().AutoCrop(cropDefinition);
            }

            return this;
        }
        
        public CropperPropertyBuilder SetAltText(string altText) {
            _altText = altText;

            return this;
        }

        public CropperPropertyBuilder SetImage(string mediaId) {
            _src = _mediaFileManager.GetSourceFile(mediaId);

            if (_src == null) {
                throw new Exception($"No media found with ID {mediaId}");
            }

            _mediaId = mediaId;

            using (var stream = _mediaFileManager.FileSystem.OpenFile(_src)) {
                var metadata = stream.GetImageMetadata();

                _height = metadata.Height;
                _width = metadata.Width;
            }

            return this;
        }

        public CropperPropertyBuilder SetImage(Blob blob) {
            return SetImage(blob.Stream, blob.Filename);
        }
        
        public CropperPropertyBuilder SetImage(byte[] bytes, string filename) {
            using (var stream = new MemoryStream(bytes)) {
                SetImage(stream, filename);

                return this;
            }
        }

        public CropperPropertyBuilder SetImage(Stream stream, string filename) {
            var instant = _clock.GetCurrentInstant();

            stream.Rewind();

            if (!_mediaFileManager.FileSystem.FileExists(filename.GetStoragePath(instant))) {
                _mediaFileManager.FileSystem.AddFile(filename.GetStoragePath(instant), stream, false);
            }

            stream.Rewind();

            var metadata = stream.GetImageMetadata();
            _height = metadata.Height;
            _width = metadata.Width;
            _filename = filename;
            _mediaId = instant.GetMediaId();
            _src = filename.GetMediaUrlPath(instant);

            return this;
        }

        public override object Build() {
            Validate();
            
            var cropperSource = new CropperSource();
            cropperSource.Filename = _filename;
            cropperSource.Height = _height.GetValueOrThrow();
            cropperSource.Width = _width.GetValueOrThrow();
            cropperSource.Src = _src;
            cropperSource.AltText = _altText;
            cropperSource.MediaId = _mediaId;
            cropperSource.Crops = _cropBuilders.Select(x => x.Build()).ToList();

            Value = JsonConvert.SerializeObject(cropperSource);

            return Value;
        }

        private void Validate() {
            if (!_src.HasValue()) {
                throw new Exception("Image must be specified");
            }
            
            if (_cropBuilders.None()) {
                throw new Exception("At least one crop must be defined");
            }
        }
    }
}