using N3O.Umbraco.Cropper.DataTypes;

namespace N3O.Umbraco.Cropper.Content;

public interface ICropBuilder {
    ICropBuilder AutoCrop(CropDefinition cropDefinition);
    ICropBuilder AutoCrop(int width, int height);
    ICropBuilder CropTo(int x, int y, int width, int height);
    CropperSource.Crop Build();
}
