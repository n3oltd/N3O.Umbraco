namespace N3O.Umbraco.Data.Models;

public class ImportTemplate : Value {
    public ImportTemplate(string filename, byte[] contents) {
        Filename = filename;
        Contents = contents;
    }

    public string Filename { get; }
    public byte[] Contents { get; }
}
