using Humanizer.Bytes;

namespace N3O.Umbraco.Cdn.Cloudflare.Models;

public interface IStreamsVideoUpload {
    string Id { get; }
    string StreamUrl { get; }
    string ThumbnailUrl { get; }
    ByteSize Size { get; }
}