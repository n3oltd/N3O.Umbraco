using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Accounts.Models {
    public class AddressReqValidator : ModelValidator<AddressReq> {
        private const int AddressFieldMaxLength = 80;
    
        public AddressReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.Line1)
                .NotEmpty()
                .WithMessage(Get<Strings>(x => x.SpecifyLine1));

            RuleFor(x => x.Line1)
                .Length(1, AddressFieldMaxLength)
                .When(x => x.Line1.HasValue())
                .WithMessage(Get<Strings>(x => x.AddressLineInvalidLength));

            RuleFor(x => x.Line2)
                .Length(0, AddressFieldMaxLength)
                .WithMessage(Get<Strings>(x => x.AddressLineInvalidLength));

            RuleFor(x => x.Line3)
                .Length(0, AddressFieldMaxLength)
                .WithMessage(Get<Strings>(x => x.AddressLineInvalidLength));

            RuleFor(x => x.Locality)
                .NotEmpty()
                .When(x => x.Country.HasValue() && !x.Country.LocalityOptional)
                .WithMessage(Get<Strings>(x => x.SpecifyLocality));

            RuleFor(x => x.Locality)
                .Length(0, AddressFieldMaxLength)
                .WithMessage(Get<Strings>(x => x.AddressLineInvalidLength));

            RuleFor(x => x.AdministrativeArea)
                .Length(0, AddressFieldMaxLength)
                .WithMessage(Get<Strings>(x => x.AddressLineInvalidLength));

            RuleFor(x => x.PostalCode)
                .NotEmpty()
                .When(x => x.Country.HasValue() && !x.Country.PostalCodeOptional)
                .WithMessage(Get<Strings>(x => x.SpecifyPostalCode));

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
            public string SpecifyLine1 => "Please specify the first line of your address";
            public string SpecifyLocality => "Please specify the town / city portion of your address";
            public string SpecifyPostalCode => "Please specify the postcode for your address";
        }
    }
}
