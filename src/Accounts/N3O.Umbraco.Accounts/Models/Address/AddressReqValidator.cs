using FluentValidation;
using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Accounts.Models;

public class AddressReqValidator : ModelValidator<AddressReq> {
    private const int AddressFieldMaxLength = 80;

    public AddressReqValidator(IFormatter formatter, IContentCache contentCache) : base(formatter) {
        var settings = contentCache.Single<AddressDataEntrySettingsContent>();
        
        RuleFor(x => x.Line1)
            .NotEmpty()
            .When(_ => settings.Line1.Required)
            .WithMessage(Get<Strings>(x => x.Specify_1, settings.Line1.Label));

        RuleFor(x => x.Line1)
            .Length(0, AddressFieldMaxLength)
            .When(x => x.Line1.HasValue())
            .WithMessage(Get<Strings>(x => x.AddressLineInvalidLength));

        RuleFor(x => x.Line2)
            .NotEmpty()
            .When(_ => settings.Line2.Required)
            .WithMessage(Get<Strings>(x => x.Specify_1, settings.Line2.Label));
        
        RuleFor(x => x.Line2)
            .Length(0, AddressFieldMaxLength)
            .WithMessage(Get<Strings>(x => x.AddressLineInvalidLength));

        RuleFor(x => x.Line3)
            .NotEmpty()
            .When(_ => settings.Line3.Required)
            .WithMessage(Get<Strings>(x => x.Specify_1, settings.Line3.Label));
        
        RuleFor(x => x.Line3)
            .Length(0, AddressFieldMaxLength)
            .WithMessage(Get<Strings>(x => x.AddressLineInvalidLength));

        RuleFor(x => x.Locality)
            .NotEmpty()
            .When(x => settings.Locality.Required &&
                       x.Country.HasValue() &&
                       !x.Country.LocalityOptional)
            .WithMessage(Get<Strings>(x => x.Specify_1, settings.Locality.Label));

        RuleFor(x => x.Locality)
            .Length(0, AddressFieldMaxLength)
            .WithMessage(Get<Strings>(x => x.AddressLineInvalidLength));

        RuleFor(x => x.AdministrativeArea)
            .NotEmpty()
            .When(_ => settings.AdministrativeArea.Required)
            .WithMessage(Get<Strings>(x => x.Specify_1, settings.AdministrativeArea.Label));
        
        RuleFor(x => x.AdministrativeArea)
            .Length(0, AddressFieldMaxLength)
            .WithMessage(Get<Strings>(x => x.AddressLineInvalidLength));

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .When(x => settings.PostalCode.Required &&
                       x.Country.HasValue() &&
                       !x.Country.PostalCodeOptional)
            .WithMessage(Get<Strings>(x => x.Specify_1, settings.PostalCode.Label));

        RuleFor(x => x.PostalCode)
            .Length(0, AddressFieldMaxLength)
            .WithMessage(Get<Strings>(x => x.AddressLineInvalidLength));

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage(Get<Strings>(x => x.SpecifyCountry));
    }

    public class Strings : ValidationStrings {
        public string AddressLineInvalidLength => $"Address line cannot exceed {AddressFieldMaxLength} characters";
        public string SpecifyCountry => "Please specify the country for your address";
        public string Specify_1 => "Please specify {0}";
    }
}
