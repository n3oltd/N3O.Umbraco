using N3O.Umbraco.Cropper;
using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Cropper.Extensions;
using N3O.Umbraco.Cropper.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public static class CrowdfunderCropperExtensions {
    public static async Task<string> GetJumboImagePath(IImageCropper imageCropper, CroppedImage croppedImage) {
        var cropperSource = croppedImage.GetUncroppedImage();
        var crop = cropperSource.GetLargestCrop();

        await imageCropper.CropAsync(JumboCropDefinition,
                                     crop,
                                     cropperSource);

        var imagePath = ImagePath.Get(cropperSource.MediaId, cropperSource.Filename, JumboCropDefinition, crop);

        return imagePath;
    }
    
    public static async Task<string> GetTallImagePathAsync(IImageCropper imageCropper, CroppedImage croppedImage) {
        var cropperSource = croppedImage.GetUncroppedImage();
        var crop = cropperSource.GetLargestCrop();

        await imageCropper.CropAsync(TallCropDefinition,
                                     crop,
                                     cropperSource);

        var imagePath = ImagePath.Get(cropperSource.MediaId, cropperSource.Filename, TallCropDefinition, crop);

        return imagePath;
    }
    
    public static async Task<string> GetWideImagePath(IImageCropper imageCropper, CroppedImage croppedImage) {
        var cropperSource = croppedImage.GetUncroppedImage();
        var crop = cropperSource.GetLargestCrop();

        await imageCropper.CropAsync(WideCropDefinition,
                                     crop,
                                     cropperSource);

        var imagePath = ImagePath.Get(cropperSource.MediaId, cropperSource.Filename, WideCropDefinition, crop);

        return imagePath;
    }
    
    private static readonly CropDefinition JumboCropDefinition = new() {
        Alias = "jumbo",
        Width = 600,
        Height = 420,
        Label = "Jumbo"
    };
    
    private static readonly CropDefinition TallCropDefinition = new() {
        Alias = "tall",
        Width = 320,
        Height = 230,
        Label = "Tall"
    };
    
    private static readonly CropDefinition WideCropDefinition = new() {
        Alias = "wide",
        Width = 200,
        Height = 200,
        Label = "Wide"
    };
}