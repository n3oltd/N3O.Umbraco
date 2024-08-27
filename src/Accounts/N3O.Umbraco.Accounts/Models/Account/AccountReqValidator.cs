using FluentValidation;
using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Accounts.Models;

public class AccountReqValidator : ModelValidator<AccountReq> {
    public AccountReqValidator(IFormatter formatter, IContentCache contentCache) : base(formatter) {
        var emailDataEntrySettings = contentCache.Single<EmailDataEntrySettingsContent>();
        var phoneDataEntrySettings = contentCache.Single<PhoneDataEntrySettingsContent>();

        RuleFor(x => x.Type)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyType));
        
        RuleFor(x => x.Individual)
            .Null()
            .When(x => x.Type != AccountTypes.Individual)
            .WithMessage(Get<Strings>(s => s.IndividualNotAllowed));
        
        RuleFor(x => x.Organization)
            .Null()
            .When(x => x.Type != AccountTypes.Organization)
            .WithMessage(Get<Strings>(s => s.OrganizationNotAllowed));
        
        RuleFor(x => x.Individual.Name)
           .NotNull()
           .When(x => x.Type == AccountTypes.Individual)
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
        public string IndividualNotAllowed => "Individual not allowed for this account type";
        public string OrganizationNotAllowed => "Organization not allowed for this account type";
        public string SpecifyAddress => "Please specify your address";
        public string SpecifyEmail => "Please specify your email";
        public string SpecifyName => "Please specify your name";
        public string SpecifyTelephone => "Please specify your telephone number";
        public string SpecifyType => "Please specify the account type";
    }
}