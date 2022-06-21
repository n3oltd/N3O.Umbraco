namespace N3O.Umbraco.Data.Models;

public class ExportFile : Value {
    public ExportFile(string filename, string contentType, byte[] contents) {
        Filename = filename;
        ContentType = contentType;
        Contents = contents;
    }

    public string Filename { get; }
    public string ContentType { get; }
    public byte[] Contents { get; }
}
