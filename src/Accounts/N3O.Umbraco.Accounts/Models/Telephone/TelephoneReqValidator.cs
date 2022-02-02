using FluentValidation;
using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Accounts.Models {
    public class TelephoneReqValidator : ModelValidator<TelephoneReq> {
        public TelephoneReqValidator(IFormatter formatter,
                                     IContentCache contentCache,
                                     IPhoneNumberValidator phoneNumberValidator)
            : base(formatter) {
            var settings = contentCache.Single<PhoneDataEntrySettingsContent>();

            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage(Get<Strings>(s => s.SpecifyCountry));
        
            RuleFor(x => x.Number)
                .NotEmpty()
                .WithMessage(Get<Strings>(x => x.SpecifyNumber));

            RuleFor(x => x.Number)
                .Must((req, x) => phoneNumberValidator.IsValid(x, req.Country))
                .When(x => settings.Validate && x.Country.HasValue() && x.Number.HasValue())
                .WithMessage(Get<Strings>(x => x.InvalidNumber));
        }

        public class Strings : ValidationStrings {
            public string InvalidNumber => "Please enter a valid telephone number so we can contact you if needed";
            public string SpecifyCountry => "Please enter the country for your telephone number";
            public string SpecifyNumber => "Please enter your telephone number so we can contact you if needed";
        }
    }
}
