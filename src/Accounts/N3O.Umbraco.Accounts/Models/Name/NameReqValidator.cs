using FluentValidation;
using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Accounts.Models;

public class NameReqValidator : ModelValidator<NameReq> {
    private const int FirstNameMinLength = 1;
    private const int LastNameMinLength = 2;
    private const int NameMaxLength = 50;

    public NameReqValidator(IFormatter formatter, IContentCache contentCache) : base(formatter) {
        var settings = contentCache.Single<NameDataEntrySettingsContent>()?.ToDataEntrySettings();
        
        RuleFor(x => x.Title)
            .NotNull()
            .When(_ => settings?.Title.Required == true)
            .WithMessage(Get<Strings>(x => x.SpecifyTitle));

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .When(_ => settings?.FirstName.Required == true)
            .WithMessage(Get<Strings>(x => x.SpecifyFirstName));

        RuleFor(x => x.FirstName)
            .Length(FirstNameMinLength, 50)
            .When(x => x.FirstName.HasValue())
            .WithMessage(Get<Strings>(x => x.FirstNameInvalidLength));

        RuleFor(x => x.LastName)
            .NotEmpty()
            .When(_ => settings?.LastName.Required == true)
            .WithMessage(Get<Strings>(x => x.SpecifyLastName));

        RuleFor(x => x.LastName)
            .Length(LastNameMinLength, NameMaxLength)
            .When(x => x.LastName.HasValue())
            .WithMessage(Get<Strings>(x => x.LastNameInvalidLength));
    }

    public class Strings : ValidationStrings {
        public string FirstNameInvalidLength => $"Please specify your full first name, not exceeding {NameMaxLength} characters";
        public string LastNameInvalidLength => $"Please specify your full last name, not exceeding {NameMaxLength} characters";
        public string SpecifyFirstName => "Please specify your first name";
        public string SpecifyLastName => "Please specify your last name";
        public string SpecifyTitle => "Please specify your title";
    }
}
