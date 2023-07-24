using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Accounts.Models;

public class TaxStatusReqValidator : ModelValidator<TaxStatusReq> {
    public TaxStatusReqValidator(IFormatter formatter)
        : base(formatter) {
        RuleFor(x => x.TaxStatus)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyTaxStatus));
    }

    public class Strings : ValidationStrings {
        public string SpecifyTaxStatus => "Please specify your tax status";
    }
}