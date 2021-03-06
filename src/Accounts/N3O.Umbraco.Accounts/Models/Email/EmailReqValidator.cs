using FluentValidation;
using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Accounts.Models;

public class EmailReqValidator : ModelValidator<EmailReq> {
    public EmailReqValidator(IFormatter formatter, IContentCache contentCache) : base(formatter) {
        var settings = contentCache.Single<EmailDataEntrySettingsContent>();
    
        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage(Get<Strings>(x => x.SpecifyAddress));

        RuleFor(x => x.Address)
            .EmailAddress()
            .When(x => settings.Validate && x.Address.HasValue())
            .WithMessage(Get<Strings>(x => x.InvalidAddress));
    }

    public class Strings : ValidationStrings {
        public string InvalidAddress => "Please enter a valid email address";
        public string SpecifyAddress => "Please enter your email address";
    }
}
