using System.IO;

namespace N3O.Umbraco.Data.Models;

public class ExportFile {
    public ExportFile(string filename, string contentType, Stream contents) {
        Filename = filename;
        ContentType = contentType;
        Contents = contents;
    }

    public string Filename { get; }
    public string ContentType { get; }
    public Stream Contents { get; }
}
