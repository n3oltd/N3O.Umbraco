using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Plugins.Lookups;
using System.Linq;

namespace N3O.Umbraco.Plugins.Models;

public class FileUploadReqValidator : ModelValidator<FileUploadReq> {
    public FileUploadReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.File)
           .Must((req, x) => ValidateAllowedExtensions(req.AllowedExtensions, x))
           .WithMessage(Get<Strings>(s => s.InvalidExtension));
        
        RuleFor(x => x.File)
           .Must((req, x) => ValidateImageOnly(req.ImagesOnly, x))
           .WithMessage(Get<Strings>(s => s.InvalidImage));
    }

    private bool ValidateImageOnly(bool? imagesOnly, IFormFile formFile) {
        if (imagesOnly == true) {
            var allowedFormats = ImageFormats.GetAllFormats().ToList();
            
            return allowedFormats.Any(x => formFile.FileName.EndsWith(x.Id));
        }

        return true;
    }

    private bool ValidateAllowedExtensions(string allowedExtensions, IFormFile fileUpload) {
        return allowedExtensions.Split(',').Any(fileUpload.FileName.EndsWith);
    }
    
    public class Strings : ValidationStrings {
        public string InvalidExtension => "The specified file has invalid extension";
        public string InvalidImage => "The specified file is not an image";
    }
}