using Humanizer.Bytes;
using N3O.Umbraco.Cdn.Cloudflare.Models;
using Newtonsoft.Json;

namespace N3O.Umbraco.Cdn.Cloudflare.Clients;

public class ApiUploadResult : IStreamsVideoUpload {
    [JsonProperty("uid")]
    public string UId { get; set; }
    
    [JsonProperty("playback")]
    public ApiPlayback Playback { get; set; }

    [JsonProperty("thumbnail")]
    public string Thumbnail { get; set; }

    [JsonProperty("size")]
    public long Size { get; set; }

    [JsonIgnore]
    string IStreamsVideoUpload.Id => UId;
    
    [JsonIgnore]
    string IStreamsVideoUpload.StreamUrl => Playback.Hls;
    
    [JsonIgnore]
    string IStreamsVideoUpload.ThumbnailUrl => Thumbnail;

    [JsonIgnore]
    ByteSize IStreamsVideoUpload.Size => ByteSize.FromBytes(Size);
}

