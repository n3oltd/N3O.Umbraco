using Humanizer.Bytes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Cdn.Cloudflare.Models;

public class StreamsVideoUpload : Value, IStreamsVideoUpload {
    [JsonConstructor]
    public StreamsVideoUpload(string id, string streamUrl, string thumbnailUrl, ByteSize size) {
        Id = id;
        StreamUrl = streamUrl;
        ThumbnailUrl = thumbnailUrl;
        Size = size;
    }

    public StreamsVideoUpload(IStreamsVideoUpload streamsVideoUpload)
        : this(streamsVideoUpload.Id,
               streamsVideoUpload.StreamUrl,
               streamsVideoUpload.ThumbnailUrl,
               streamsVideoUpload.Size) { }

    public string Id { get; }
    public string StreamUrl { get; }
    public string ThumbnailUrl { get; }
    public ByteSize Size { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
        yield return StreamUrl;
        yield return ThumbnailUrl;
        yield return Size;
    }
}