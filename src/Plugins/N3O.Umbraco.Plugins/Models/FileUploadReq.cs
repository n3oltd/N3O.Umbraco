using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Plugins.Models {
    public class FileUploadReq : ImageUploadReq {
        [Name("Allowed Extensions")]
        public string AllowedExtensions { get; set; }
        
        [Name("Images Only")]
        public bool? ImagesOnly { get; set; }
        
        [Name("Max File Size MB")]
        public double? MaxFileSizeMb { get; set; }
        
        [Name("Max Height")]
        public int? MaxHeight { get; set; }
    
        [Name("Max Width")]
        public int? MaxWidth { get; set; }
    }
}