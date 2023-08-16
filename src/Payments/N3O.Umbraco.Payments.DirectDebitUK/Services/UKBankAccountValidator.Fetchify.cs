using N3O.Umbraco.Payments.DirectDebitUK.Clients.Fetchify;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.DirectDebitUK; 

public class FetchifyUKBankAccountValidator : IUKBankAccountValidator {
    private readonly IFetchifyApiClient _client;

    public FetchifyUKBankAccountValidator(IFetchifyApiClient client) {
        _client = client;
    }
    
    public async Task<bool> IsValidAsync(string sortCode, string accountNumber) {
        var result = await _client.ValidateAsync(accountNumber, sortCode);

        return result.Successful;
    }
}