using N3O.Umbraco.Cropper.DataTypes;

namespace N3O.Umbraco.Cropper.Content {
    public interface ICropBuilder {
        void CropTo(int x, int y, int width, int height);
        CropperSource.Crop Build();
    }
}