using Humanizer.Bytes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Json;
using N3O.Umbraco.Utilities;
using Newtonsoft.Json;

namespace N3O.Umbraco {
    [StringSchema("A valid storage token")]
    public sealed class StorageToken {
        public StorageToken(string filename, string storageFolderName, string contentType, ByteSize size) {
            Filename = filename;
            ContentType = contentType;
            Size = size;
        }

        public string Filename { get; }
        public string StorageFolderName { get; }
        public string ContentType { get; }
        public ByteSize Size { get; }

        public string ToBase64String() {
            var json = JsonConvert.SerializeObject(this, new ByteSizeJsonConverter());

            var base64EncodedData = Base64.Encode(json);

            return base64EncodedData;
        }

        public static StorageToken FromBase64String(string base64EncodedData) {
            var json = Base64.Decode(base64EncodedData);

            if (json == null) {
                return null;
            }

            var storageToken = JsonConvert.DeserializeObject<StorageToken>(json, new ByteSizeJsonConverter());

            return storageToken;
        }

        public static StorageToken FromBlob(Blob blob) {
            return new StorageToken(blob.Filename, blob.StorageFolderName, blob.ContentType, blob.Size);
        }
    }
}