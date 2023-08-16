using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.DirectDebitUK.Clients.Loqate;

public interface ILoqateApiClient {
    [Get("/Validate/v2/json3.ws")]
    Task<ValidateResponse> ValidateAsync([Query] [AliasAs("AccountNumber")] string accountNumber,
                                         [Query] [AliasAs("SortCode")] string sortCode);
}