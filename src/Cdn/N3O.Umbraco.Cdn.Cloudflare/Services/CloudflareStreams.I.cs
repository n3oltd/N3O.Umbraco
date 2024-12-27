using N3O.Umbraco.Cdn.Cloudflare.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cdn.Cloudflare;

public interface ICloudflareStreams {
    Task<StreamsVideoUpload> UploadVideoAsync(string url, string name);
}