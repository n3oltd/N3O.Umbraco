using N3O.Umbraco.Plugins.Lookups;
using System.IO;

namespace N3O.Umbraco.Plugins.Models;

public class UploadedImage : UploadedFile {
    public UploadedImage(Stream stream, string contentType, string filename, ImageMetadata metadata)
        : base(stream, contentType, filename) {
        Metadata = metadata;
    }

    public UploadedImage(UploadedFile file, ImageMetadata metadata)
        : this(file.Stream, file.ContentType, file.Filename, metadata) { }

    public UploadedImage(UploadedFile file, ImageFormat format, int height, int width)
        : this(file, new ImageMetadata(format, height, width)) { }

    public ImageMetadata Metadata { get; }
}
