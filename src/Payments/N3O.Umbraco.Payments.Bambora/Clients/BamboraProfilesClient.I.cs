using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Bambora.Clients;

public interface IBamboraProfilesClient {
    [Post("/profiles")]
    Task<ApiProfileRes> CreateProfileAsync(ApiProfileReq req);
}
