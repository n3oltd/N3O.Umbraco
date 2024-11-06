using N3O.Umbraco.Cropper;
using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Cropper.Extensions;
using N3O.Umbraco.Cropper.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public static class ImageCropperExtensions {
    public static async Task<string> GetImagePathAsync(this IImageCropper imageCropper,
                                                       CroppedImage croppedImage,
                                                       CropDefinition cropDefinition) {
        var cropperSource = croppedImage.GetUncroppedImage();
        var crop = cropperSource.GetLargestCrop();

        await imageCropper.CropAsync(cropDefinition, crop, cropperSource);

        var imagePath = ImagePath.Get(cropperSource.MediaId, cropperSource.Filename, cropDefinition, crop);

        return imagePath;
    }
    
    public static readonly CropDefinition JumboCropDefinition = new() {
        Alias = "jumbo",
        Width = 600,
        Height = 420,
        Label = "Jumbo"
    };
    
    public static readonly CropDefinition TallCropDefinition = new() {
        Alias = "tall",
        Width = 320,
        Height = 230,
        Label = "Tall"
    };
    
    public static readonly CropDefinition WideCropDefinition = new() {
        Alias = "wide",
        Width = 200,
        Height = 200,
        Label = "Wide"
    };
}