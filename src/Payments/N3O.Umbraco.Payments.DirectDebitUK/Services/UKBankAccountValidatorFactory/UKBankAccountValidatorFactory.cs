using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Payments.DirectDebitUK;

public class UKBankAccountValidatorFactory : IUKBankAccountValidatorFactory {
    private readonly IEnumerable<IUKBankAccountValidator> _bankAccountValidators;

    public UKBankAccountValidatorFactory(IEnumerable<IUKBankAccountValidator> bankAccountValidators) {
        _bankAccountValidators = bankAccountValidators;
    }
    
    public IUKBankAccountValidator CreateValidator() {
        var validator = _bankAccountValidators.FirstOrDefault(x => x.HasConfiguration);

        return validator;
    }
}