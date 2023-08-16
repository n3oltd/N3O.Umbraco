using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.DirectDebitUK.Clients.Fetchify; 

public interface IFetchifyApiClient {
    [Get("/1.1/validate")]
    Task<ValidateResponse> ValidateAsync([Query] [AliasAs("accountNumber")] string accountNumber,
                                         [Query] [AliasAs("sortCode")] string sortCode);
}