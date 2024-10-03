using N3O.Umbraco.Cropper.DataTypes;
using System.Linq;

namespace N3O.Umbraco.Cropper.Extensions;

public static class CropperSourceExtensions {
    public static CropperSource.Crop GetLargestCrop(this CropperSource cropperSource) {
        return cropperSource.Crops.OrderByDescending(c => c.Width * c.Height).First();
    }
}