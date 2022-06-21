using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.GoCardless.Models;

public class RedirectFlowReqValidator : ModelValidator<RedirectFlowReq> {
    public RedirectFlowReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.ReturnUrl)
            .NotEmpty()
            .WithMessage(Get<Strings>(x => x.SpecifyReturnUrl));
    }

    public class Strings : ValidationStrings {
        public string SpecifyReturnUrl => "Please specify the return URL";
    }
}
