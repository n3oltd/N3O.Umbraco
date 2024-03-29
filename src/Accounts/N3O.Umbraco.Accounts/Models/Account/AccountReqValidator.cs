using FluentValidation;
using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Accounts.Models;

public class AccountReqValidator : ModelValidator<AccountReq> {
    public AccountReqValidator(IFormatter formatter, IContentCache contentCache) : base(formatter) {
        var emailDataEntrySettings = contentCache.Single<EmailDataEntrySettingsContent>();
        var phoneDataEntrySettings = contentCache.Single<PhoneDataEntrySettingsContent>();

        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyName));
    
        RuleFor(x => x.Address)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyAddress));

        RuleFor(x => x.Email)
            .NotNull()
            .When(_ => emailDataEntrySettings.Required)
            .WithMessage(Get<Strings>(s => s.SpecifyEmail));

        RuleFor(x => x.Telephone)
            .NotNull()
            .When(_ => phoneDataEntrySettings.Required)
            .WithMessage(Get<Strings>(s => s.SpecifyTelephone));
    }

    public class Strings : ValidationStrings {
        public string SpecifyAddress => "Please specify your address";
        public string SpecifyEmail => "Please specify your email";
        public string SpecifyName => "Please specify your name";
        public string SpecifyTelephone => "Please specify your telephone number";
    }
}
