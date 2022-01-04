using N3O.Umbraco.Uploader.DataTypes;

namespace N3O.Umbraco.Uploader.Models {
    public class FileUpload : Value {
        public FileUpload(UploaderSource source) {
            AltText = source.AltText;
            Extension = source.Extension;
            Filename = source.Filename;
            SizeMb = source.SizeMb;
            UrlPath = source.UrlPath;
        }
    
        public string AltText { get; }
        public string Extension { get; }
        public string Filename { get; }
        public double SizeMb { get; }
        public string UrlPath { get; }
    }
}
