using N3O.Umbraco.Cropper.DataTypes;

namespace N3O.Umbraco.Cropper.Content {
    public class CropBuilder : ICropBuilder {
        private int _x;
        private int _y;
        private int _height;
        private int _width;

        public void CropTo(int x, int y, int width, int height) {
            _x = x;
            _y = y;
            _height = height;
            _width = width;
        }

        public CropperSource.Crop Build() {
            var crop = new CropperSource.Crop();
            crop.Height = _height;
            crop.Width = _width;
            crop.X = _x;
            crop.Y = _y;

            return crop;
        }
    }
}