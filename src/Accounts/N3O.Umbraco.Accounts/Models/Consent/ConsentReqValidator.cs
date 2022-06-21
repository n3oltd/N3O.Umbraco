using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Accounts.Models;

public class ConsentReqValidator : ModelValidator<ConsentReq> {
    public ConsentReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Choices)
            .NotEmpty()
            .WithMessage(Get<Strings>(s => s.SpecifyChoices));
    }

    public class Strings : ValidationStrings {
        public string SpecifyChoices => "Please specify your preference choices";
    }
}
