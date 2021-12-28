using System.IO;
using Microsoft.Net.Http.Headers;
using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Plugins {
    public static class MultipartRequestHelper {
        // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        // The spec says 70 characters is a reasonable limit.
        public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit) {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

            if (!boundary.HasValue()) {
                throw new InvalidDataException("Missing content-type boundary");
            }

            if (boundary.Length > lengthLimit) {
                throw new InvalidDataException($"Multipart boundary length limit {lengthLimit} exceeded");
            }

            return boundary;
        }

        public static bool IsMultipartContentType(string contentType) {
            return contentType.HasValue() &&
                   contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition) {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null &&
                   contentDisposition.DispositionType.Equals("form-data") &&
                   contentDisposition.FileName.Value.HasValue() &&
                   contentDisposition.FileNameStar.Value.HasValue();
        }

        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition) {
            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
            return contentDisposition != null &&
                   contentDisposition.DispositionType.Equals("form-data") &&
                   (contentDisposition.FileName.Value.HasValue() || contentDisposition.FileNameStar.Value.HasValue());
        }
    }
}
