using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.DirectDebitUK; 

public interface IUKBankAccountValidator {
    bool CanValidate();
    Task<bool> IsValidAsync(string sortCode, string accountNumber);
}