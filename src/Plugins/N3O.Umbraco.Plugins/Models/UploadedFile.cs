using System;
using System.IO;

namespace N3O.Umbraco.Plugins.Models;

public class UploadedFile : IDisposable {
    public UploadedFile(Stream stream, string contentType, string filename) {
        Stream = stream;
        ContentType = contentType;
        Filename = filename;
    }

    public Stream Stream { get; }
    public string ContentType { get; }
    public string Filename { get; }
    public long Bytes => Stream?.Length ?? 0;
    
    public void Dispose() {
        Stream?.Dispose();
    }
}
