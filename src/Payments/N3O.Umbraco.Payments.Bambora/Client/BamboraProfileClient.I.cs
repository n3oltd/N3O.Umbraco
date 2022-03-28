using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public interface IBamboraProfileClient {
        [Post("/profiles")]
        Task<ApiProfileRes> CreateProfileAsync(ApiProfileReq req);
    }
}