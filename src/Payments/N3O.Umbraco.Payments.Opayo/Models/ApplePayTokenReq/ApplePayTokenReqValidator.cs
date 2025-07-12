using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Opayo.Models;

public class ApplePayTokenReqValidator : ModelValidator<ApplePayTokenReq> {
    public ApplePayTokenReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.ApplicationData)
           .NotEmpty()
           .WithMessage(Get<Strings>(x => x.SpecifyApplicationData));

        RuleFor(x => x.DisplayName)
           .NotEmpty()
           .WithMessage(Get<Strings>(x => x.SpecifyDisplayName));

        RuleFor(x => x.PaymentData)
           .NotEmpty()
           .WithMessage(Get<Strings>(x => x.SpecifyPaymentData));

        RuleFor(x => x.SessionValidationToken)
           .NotEmpty()
           .WithMessage(Get<Strings>(x => x.SpecifySessionValidationToken));
    }

    public class Strings : ValidationStrings {
        public string SpecifyApplicationData => "The Application Data must be specified in the Apple Pay Token";
        public string SpecifyDisplayName => "The Display Name must be specified in the Apple Pay Token";
        public string SpecifyPaymentData => "The Payment Data must be specified in the Apple Pay Token";
        public string SpecifySessionValidationToken => "The Session Validation Token must be specified in the Apple Pay Token";
    }
}