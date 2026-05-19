using Humanizer.Bytes;
using System.Collections.Generic;
using System.IO;

namespace N3O.Umbraco;

public class Blob : Value {
    public Blob(string filename, string storageFolderName, string contentType, ByteSize size, Stream stream) {
        Filename = filename;
        StorageFolderName = storageFolderName;
        ContentType = contentType;
        Size = size;
        Stream = stream;
    }
    
    public string Filename { get; }
    public string StorageFolderName { get; }
    public string ContentType { get; }
    public ByteSize Size { get; }
    public Stream Stream { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Filename;
        yield return StorageFolderName;
        yield return ContentType;
        yield return Size;
    }
}
