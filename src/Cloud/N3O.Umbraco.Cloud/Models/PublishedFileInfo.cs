using N3O.Umbraco.Cloud.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedFileInfo : Value {
    public PublishedFileInfo(PublishedFileKind kind, string path) {
        Kind = kind;
        Path = path;
    }

    public PublishedFileKind Kind { get; }
    public string Path { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Kind;
        yield return Path;
    }

    public static PublishedFileInfo Create(PublishedFileKind kind, string path) {
        return new PublishedFileInfo(kind, path);
    }
    
    public static PublishedFileInfo ForRootFile(PublishedFileKind kind, string filename) {
        return new PublishedFileInfo(kind, $"{kind.Id}/{filename}.json");
    }
}