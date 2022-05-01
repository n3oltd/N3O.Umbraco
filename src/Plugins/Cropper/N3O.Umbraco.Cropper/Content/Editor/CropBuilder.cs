using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Cropper.Content {
    public class CropBuilder : ICropBuilder {
        private readonly int _imageWidth;
        private readonly int _imageHeight;
        private int? _x;
        private int? _y;
        private int? _height;
        private int? _width;

        public CropBuilder(int imageWidth, int imageHeight) {
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;
        }

        public void AutoCrop(CropDefinition cropDefinition) {
            var x = Math.Max(0, (_imageWidth - cropDefinition.Width) / 2m);
            var y = Math.Max(0, (_imageHeight - cropDefinition.Height) / 2m);

            CropTo((int) x,
                   (int) y,
                   Math.Min(cropDefinition.Width, _imageWidth),
                   Math.Min(cropDefinition.Height, _imageHeight));
        }

        public void CropTo(int x, int y, int width, int height) {
            _x = x;
            _y = y;
            _height = height;
            _width = width;
        }

        public CropperSource.Crop Build() {
            Validate();
            
            var crop = new CropperSource.Crop();
            crop.Height = _height.GetValueOrThrow();
            crop.Width = _width.GetValueOrThrow();
            crop.X = _x.GetValueOrThrow();
            crop.Y = _y.GetValueOrThrow();

            return crop;
        }

        private void Validate() {
            if (_x == null || _y == null || _height == null || _width == null) {
                throw new Exception("Crop dimensions must be specified");
            }

            if ((_x + _width.Value) > _imageWidth || (_y + _height.Value) > _imageHeight) {
                throw new Exception("Crop dimensions are invalid");
            }
        }
    }
}