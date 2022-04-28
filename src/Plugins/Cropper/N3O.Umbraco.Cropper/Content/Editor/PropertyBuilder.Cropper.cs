using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.DataTypes;
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
        public List<ICropBuilder> _cropBuilders = new();
        private string _src;
        private string _mediaId;
        private string _filename;
        private int _width;
        private int _height;
        private string _altText;

        public CropperPropertyBuilder(MediaFileManager mediaFileManager, ILocalClock clock) {
            _mediaFileManager = mediaFileManager;
            _clock = clock;
        }

        public ICropBuilder AddCrop() {
            var cropBuilder = new CropBuilder();

            _cropBuilders.Add(cropBuilder);

            return cropBuilder;
        }

        public CropperPropertyBuilder AutoCrop(CropperConfiguration configuration) {
            foreach (var cropDefinition in configuration.CropDefinitions) {
                AddAutoCrop(cropDefinition);
            }

            return this;
        }

        private void AddAutoCrop(CropDefinition cropDefinition) {
            var x = (_height - cropDefinition.Height) / 2;
            var y = ((_width) - cropDefinition.Width) / 2;
            AddCrop().CropTo(x, y, cropDefinition.Width, cropDefinition.Height);
        }

        public CropperPropertyBuilder SetImage(string mediaId) {
            _src = _mediaFileManager.FileSystem
                                    .GetFiles(mediaId, "*.*")
                                    .OrderBy(x => _mediaFileManager.FileSystem.GetLastModified(x))
                                    .FirstOrDefault();

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

        public CropperPropertyBuilder SetImage(Stream stream, string filename) {
            var instant = _clock.GetCurrentInstant();
            _src = filename.GetStoragePath(instant);

            stream.Seek(0, SeekOrigin.Begin);
            _mediaFileManager.FileSystem.AddFile(_src, stream, false);

            stream.Seek(0, SeekOrigin.Begin);

            var metadata = stream.GetImageMetadata();
            _height = metadata.Height;
            _width = metadata.Width;
            _filename = filename;
            _mediaId = Path.GetDirectoryName(_src);

            return this;
        }

        public CropperPropertyBuilder SetImage(byte[] bytes, string filename) {
            SetImage(new MemoryStream(bytes), filename);

            return this;
        }

        public CropperPropertyBuilder SetAltText(string altText) {
            _altText = altText;

            return this;
        }

        public override object Build() {
            var cropperSource = new CropperSource();
            cropperSource.Filename = _filename;
            cropperSource.Height = _height;
            cropperSource.Width = _width;
            cropperSource.Src = _src;
            cropperSource.AltText = _altText;
            cropperSource.MediaId = _mediaId;
            cropperSource.Crops = _cropBuilders.Select(x => x.Build()).ToList();

            Value = JsonConvert.SerializeObject(cropperSource);

            return Value;
        }
    }
}