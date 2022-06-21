using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Email.Models;

public class EmailIdentityReqValidator : ModelValidator<EmailIdentityReq> {
    private const int EmailMaxLength = 100;
    private const int NameMaxLength = 100;

    public EmailIdentityReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(Get<Strings>(s => s.SpecifyEmail));

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => x.Email.HasValue())
            .WithMessage(Get<Strings>(s => s.EmailInvalid));

        RuleFor(x => x.Email)
            .MaximumLength(EmailMaxLength)
            .When(x => x.Email.HasValue())
            .WithMessage(Get<Strings>(s => s.EmailTooLong_1, EmailMaxLength));

        RuleFor(x => x.Name)
            .MaximumLength(NameMaxLength)
            .When(x => x.Name.HasValue())
            .WithMessage(Get<Strings>(s => s.NameTooLong_1, NameMaxLength));
    }
}

public class Strings : ValidationStrings {
    public string EmailInvalid => "The specified email address is invalid";
    public string EmailTooLong_1 => "Email address exceeds maximum length of {0} characters";
    public string NameTooLong_1 => "Name exceeds maximum length of {0} characters";
    public string SpecifyEmail => "Email must be specified";
}
