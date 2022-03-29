using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public interface IBamboraProfilesClient {
        [Post("/profiles")]
        Task<ApiProfileRes> CreateProfileAsync(ApiProfileReq req);
    }
}