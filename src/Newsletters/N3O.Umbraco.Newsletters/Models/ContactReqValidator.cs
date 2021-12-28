using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Newsletters.Models;

public class ContactReqValidator : ModelValidator<ContactReq> {
    public ContactReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(Get<Strings>(x => x.Specify));
        
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => x.Email.HasValue())
            .WithMessage(Get<Strings>(x => x.InvalidEmail));
    }

    public class Strings : ValidationStrings {
        public string Specify => "Please specify your email address";
        public string InvalidEmail => "The specified email address is invalid";
    }
}
