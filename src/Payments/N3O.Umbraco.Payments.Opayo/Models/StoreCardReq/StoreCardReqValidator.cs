using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Opayo.Models;

public class StoreCardReqValidator : ModelValidator<StoreCardReq> {
    public StoreCardReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.AdvancePayment)
           .NotEmpty()
           .WithMessage(Get<Strings>(x => x.SpecifyCallbackUrl));
    }

    public class Strings : ValidationStrings {
        public string SpecifyCallbackUrl => "Please specify the advance payment";
    }
}
