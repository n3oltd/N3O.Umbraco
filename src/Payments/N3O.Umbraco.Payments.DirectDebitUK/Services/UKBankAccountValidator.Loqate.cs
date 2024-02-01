using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.DirectDebitUK.Clients.Loqate;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.DirectDebitUK; 

public class LoqateUKBankAccountValidator : IUKBankAccountValidator {
    private readonly ILoqateApiClient _client;

    public LoqateUKBankAccountValidator(ILoqateApiClient client = null) {
        _client = client;
    }

    public bool CanValidate() {
        return _client.HasValue();
    }

    public async Task<bool> IsValidAsync(string sortCode, string accountNumber) {
        var result = await _client.ValidateAsync(accountNumber, sortCode);
        
        return result.Items.Single().IsCorrect;
    }
}