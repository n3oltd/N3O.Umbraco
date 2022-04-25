using Humanizer.Bytes;
using System.Collections.Generic;
using System.IO;

namespace N3O.Umbraco {
    public class Blob : Value {
        public Blob(string filename, ByteSize size, string contentType, Stream stream) {
            Filename = filename;
            Size = size;
            ContentType = contentType;
            Stream = stream;
        }

        public string Filename { get; }
        public ByteSize Size { get; }
        public string ContentType { get; }
        public Stream Stream { get; }

        protected override IEnumerable<object> GetAtomicValues() {
            yield return Filename;
            yield return Size;
            yield return ContentType;
        }
    }
}