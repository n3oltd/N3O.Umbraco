using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Plugins.Lookups;
using N3O.Umbraco.Plugins.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System.IO;
using System.Threading.Tasks;

namespace N3O.Umbraco.Plugins.Controllers {
    public partial class PluginController {
        private static readonly int MaxSizeMb = 100 * 1024 * 1024;
        private static readonly Size MaxDimensions = new(10000, 10000);
    
        protected async Task<UploadedImage> GetUploadedImageAsync(ImageUploadReq req) {
            try {
                using (var reqStream = req.File.OpenReadStream()) {
                    var fileStream = new MemoryStream();

                    await reqStream.CopyToAsync(fileStream);

                    fileStream.Rewind();

                    var uploadedFile = new UploadedFile(fileStream, req.File.ContentDisposition, req.File.FileName);
                    var metadata = GetImageMetadata(fileStream);

                    var uploadedImage = new UploadedImage(uploadedFile, metadata);

                    if (!SizeAndDimensionsAreValid(uploadedImage, req.MinHeight, req.MinWidth)) {
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
                var metadata = new ImageMetadata(ImageFormat.From(format), image.Height, image.Width);

                stream.Rewind();
                
                return metadata;
            }
        }

        private bool SizeAndDimensionsAreValid(UploadedImage uploadedImage,
                                               int? minHeight,
                                               int? minWidth,
                                               int? maxHeight = null,
                                               int? maxWidth = null,
                                               double? maxSizeMb = null) {
            var height = uploadedImage.Metadata.Height;
            var width = uploadedImage.Metadata.Width;

            if (uploadedImage.Bytes > (maxSizeMb ?? MaxSizeMb) * 1024 * 1024) {
                return false;
            }
        
            if (height > (maxHeight ?? MaxDimensions.Height) || height < minHeight) {
                return false;
            }
        
            if (width > (maxWidth ?? MaxDimensions.Width) || width < minWidth) {
                return false;
            }

            return true;
        }
    }
}
