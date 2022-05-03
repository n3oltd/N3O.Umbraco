using HeyRed.Mime;

namespace N3O.Umbraco.Utilities {
    public static class FileUtility {
        public static string GetContentType(string filename) {
            try {
                return MimeTypesMap.GetMimeType(filename);
            } catch {
                return "application/octet-stream";
            }
        }
        
        public static string GetFileExtension(string contentType) {
            try {
                return MimeTypesMap.GetExtension(contentType);
            } catch {
                return "";
            }
        }
    }
}