using System;
using System.IO;

namespace N3O.Umbraco.Extensions {
    public static class StreamExtensions {
        public static void Rewind(this Stream stream) {
            if (!stream.CanSeek && stream.Position != 0) {
                throw new Exception("Stream does not support seeking");
            }

            stream.Seek(0, SeekOrigin.Begin);
        }
    }
}