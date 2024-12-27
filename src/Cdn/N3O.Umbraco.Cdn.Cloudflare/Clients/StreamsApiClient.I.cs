using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cdn.Cloudflare.Clients;

public interface IStreamsApiClient {
    [Post("stream/copy")]
    Task<ApiResponse<ApiUploadRes>> CopyVideoAsync([Body] ApiUploadReq request);
}