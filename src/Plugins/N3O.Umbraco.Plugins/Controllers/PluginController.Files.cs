using N3O.Umbraco.Plugins.Models;
using System.IO;
using System.Threading.Tasks;

namespace N3O.Umbraco.Plugins.Controllers {
    public partial class PluginController {
        protected async Task<UploadedFile> GetUploadedFileAsync(FileUploadReq req) {
            try {
                using (var reqStream = req.File.OpenReadStream()) {
                    var fileStream = new MemoryStream();

                    await reqStream.CopyToAsync(fileStream);

                    fileStream.Seek(0, SeekOrigin.Begin);

                    var uploadedFile = new UploadedFile(fileStream, req.File.ContentDisposition, req.File.FileName);

                    if (req.ImagesOnly == true) {
                        var metadata = GetImageMetadata(fileStream);

                        var uploadedImage = new UploadedImage(uploadedFile, metadata);

                        if (!SizeAndDimensionsAreValid(uploadedImage,
                                                       req.MinHeight,
                                                       req.MinWidth,
                                                       req.MaxHeight,
                                                       req.MaxWidth,
                                                       req.MaxFileSizeMb)) {
                            uploadedFile = null;
                        }
                    }

                    return uploadedFile;
                }
            } catch {
                return null;
            }
        }
    }
}
