using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Plugins.Lookups;
using N3O.Umbraco.Plugins.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System.IO;
using System.Threading.Tasks;

namespace N3O.Umbraco.Plugins.Controllers;

public partial class PluginController {
    private static readonly int MaxSizeBytes = 20 * 1024 * 1024;
    private static readonly Size MaxDimensions = new(4000, 4000);
    
    protected async Task<UploadedImage> GetUploadedImageAsync(ImageUploadReq req) {
        try {
            using (var imgStream = req.Image.OpenReadStream()) {
                var fileStream = new MemoryStream();

                await imgStream.CopyToAsync(fileStream);

                fileStream.Seek(0, SeekOrigin.Begin);

                var uploadedFile = new UploadedFile(fileStream, req.Image.ContentDisposition, req.Image.FileName);
                var metadata = GetImageMetadata(fileStream);

                var uploadedImage = new UploadedImage(uploadedFile, metadata);

                if (SizeAndDimensionsAreValid(uploadedImage, req.MinHeight, req.MinWidth)) {
                    uploadedImage = null;
                }

                return uploadedImage;
            }
        } catch {
            return null;
        }
    }

    protected ImageMetadata GetImageMetadata(Stream stream) {
        using (var image = Image.Load(stream, out var format)) {
            var metadata = new ImageMetadata(GetImageFormat(format), image.Height, image.Width);

            return metadata;
        }
    }

    private ImageFormat GetImageFormat(IImageFormat format) {
        if (format == JpegFormat.Instance) {
            return ImageFormats.Jpg;
        } else if (format == PngFormat.Instance) {
            return ImageFormats.Png;
        } else if (format == GifFormat.Instance) {
            return ImageFormats.Gif;
        } else {
            throw UnrecognisedValueException.For(format);
        }
    }
    
    private bool SizeAndDimensionsAreValid(UploadedImage uploadedImage, int? minHeight, int? minWidth) {
        var height = uploadedImage.Metadata.Height;
        var width = uploadedImage.Metadata.Width;

        if (uploadedImage.Bytes > MaxSizeBytes) {
            return false;
        }
        
        if (height > MaxDimensions.Height || height < minHeight) {
            return false;
        }
        
        if (width > MaxDimensions.Width || width < minWidth) {
            return false;
        }

        return true;
    }
}
