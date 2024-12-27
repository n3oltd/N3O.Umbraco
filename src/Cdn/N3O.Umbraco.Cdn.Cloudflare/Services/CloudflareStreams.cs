using N3O.Umbraco.Cdn.Cloudflare.Clients;
using N3O.Umbraco.Cdn.Cloudflare.Models;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cdn.Cloudflare;

public class CloudflareStreams : ICloudflareStreams {
    private readonly IStreamsApiClient _streamsApiClient;

    public CloudflareStreams(IStreamsApiClient streamsApiClient) {
        _streamsApiClient = streamsApiClient;
    }
    
    public async Task<StreamsVideoUpload> UploadVideoAsync(string url, string name) {
        var apiReq = new ApiUploadReq();
        apiReq.Url = url;
        
        apiReq.Meta = new ApiMetadataReq();
        apiReq.Meta.Name = name;

        var res = await _streamsApiClient.CopyVideoAsync(apiReq);

        if (res.Content?.Success == true) {
            return new StreamsVideoUpload(res.Content.Result);
        } else {
            if (res.HasAny(x => x.Content?.Errors)) {
                throw new Exception($"{res.Content.Errors.First().Code}: {res.Content.Errors.First().Message}");
            } else {
                throw new Exception("An unrecognised error occured");
            }
        }
    }
}