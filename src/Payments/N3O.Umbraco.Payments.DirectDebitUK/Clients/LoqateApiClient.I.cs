using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.DirectDebitUK.Clients;

public interface ILoqateApiClient {
    [Get("/Validate/v2/json3.ws")]
    Task<ValidateLoqateResponse> ValidateAsync([Query] [AliasAs("AccountNumber")] string accountNumber,
                                               [Query] [AliasAs("SortCode")] string sortCode);
}