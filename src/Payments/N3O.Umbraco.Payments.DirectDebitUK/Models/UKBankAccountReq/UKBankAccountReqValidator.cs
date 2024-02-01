using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Payments.DirectDebitUK.Models;

public class UKBankAccountReqValidator : ModelValidator<UKBankAccountReq> {
    private const int AccountHolderMaxLength = 18;
    
    private readonly IEnumerable<IUKBankAccountValidator> _bankAccountValidators;

    public UKBankAccountReqValidator(IFormatter formatter, IEnumerable<IUKBankAccountValidator> bankAccountValidators)
        : base(formatter) {
        _bankAccountValidators = bankAccountValidators.ToList();
        
        RuleFor(x => x.AccountHolder)
            .NotEmpty()
            .WithMessage(Get<Strings>(s => s.SpecifyAccountHolder));

        RuleFor(x => x.AccountNumber)
            .NotEmpty()
            .WithMessage(Get<Strings>(s => s.SpecifyAccountNumber));
        
        RuleFor(x => x.SortCode)
            .NotEmpty()
            .WithMessage(Get<Strings>(s => s.SpecifySortCode));

        RuleFor(x => x.AccountHolder)
            .MaximumLength(AccountHolderMaxLength)
            .WithMessage(Get<Strings>(s => s.AccountHolderTooLong));
        
        RuleFor(x => x.AccountNumber)
            .Must(IsAccountNumberValid)
            .When(x => x.AccountNumber.HasValue())
            .WithMessage((_, x) => Get<Strings>(s => s.AccountNumberInvalid_1, x));

        RuleFor(x => x.SortCode)
            .Must(IsSortCodeValid)
            .When(x => x.SortCode.HasValue())
            .WithMessage((_, x) => Get<Strings>(s => s.SortCodeInvalid_1, x));

        RuleFor(x => x)
            .Must(AccountIsValid)
            .When(x => x.AccountHolder.HasValue() && x.AccountHolder.Length <= AccountHolderMaxLength &&
                       x.AccountNumber.HasValue() && IsAccountNumberValid(x.AccountNumber) &&
                       x.SortCode.HasValue() && IsSortCodeValid(x.SortCode))
            .WithMessage(Get<Strings>(s => s.AccountDetailsInvalid));
    }

    private bool AccountIsValid(UKBankAccountReq req) {
        var bankAccountValidator = _bankAccountValidators.SingleOrDefault(x => x.CanValidate());
        
        if (bankAccountValidator != null) {
            var result = bankAccountValidator.IsValidAsync(req.AccountNumber, req.SortCode).GetAwaiter().GetResult();

            return result;
        } else {
            return true;
        }
    }

    private bool IsAccountNumberValid(string accountNumber) {
        if (accountNumber.Length < 6 || accountNumber.Length > 8) {
            return false;
        }

        if (!accountNumber.All(char.IsDigit)) {
            return false;
        }

        return true;
    }

    private bool IsSortCodeValid(string sortCode) {
        if (sortCode.Length != 6) {
            return false;
        }

        if (!sortCode.All(char.IsDigit)) {
            return false;
        }

        return true;
    }

    public class Strings : ValidationStrings {
        public string AccountDetailsInvalid => "The account number and/or sort code are not valid";
        public string AccountHolderTooLong => $"Account holder allows a maximum of {AccountHolderMaxLength} characters";
        public string AccountNumberInvalid_1 => $"{"{0}".Quote()} is not a valid UK account number";
        public string SortCodeInvalid_1 => $"{"{0}".Quote()} is not a valid UK sort code";
        public string SpecifyAccountHolder => "Please specify the account holder";
        public string SpecifyAccountNumber => "Please specify the account number";
        public string SpecifySortCode => "Please specify the sort code";
    }
}