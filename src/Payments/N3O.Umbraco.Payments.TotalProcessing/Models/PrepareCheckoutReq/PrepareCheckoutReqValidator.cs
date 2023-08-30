using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public class PrepareCheckoutReqValidator : ModelValidator<PrepareCheckoutReq> {
    public PrepareCheckoutReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Value)
           .NotEmpty()
           .WithMessage(Get<Strings>(x => x.SpecifyValue));

        RuleFor(x => x.ReturnUrl)
            .NotEmpty()
            .WithMessage(Get<Strings>(x => x.SpecifyReturnUrl));
    }

    public class Strings : ValidationStrings {
        public string SpecifyReturnUrl => "Please specify the return URL";
        public string SpecifyValue => "Please specify the value";
    }
}
