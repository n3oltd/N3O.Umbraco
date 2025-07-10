using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Opayo.Models;

public class ChargeCardReqValidator : ModelValidator<ChargeCardReq> {
    public ChargeCardReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x)
            .Must(x => x.CardIdentifier.HasValue() || x.GooglePayToken.HasValue() || x.ApplePayToken.HasValue())
            .WithMessage(Get<Strings>(x => x.SinglePaymentSourceRequired));
        
        RuleFor(x => x)
            .Must(x => !x.CardIdentifier.HasValue() || !x.GooglePayToken.HasValue() || !x.ApplePayToken.HasValue())
            .WithMessage(Get<Strings>(x => x.SinglePaymentSourceRequired));
        
        RuleFor(x => x.MerchantSessionKey)
            .NotEmpty()
            .WithMessage(Get<Strings>(x => x.SpecifyMerchantSessionKey));
        
        RuleFor(x => x.Value)
            .NotEmpty()
            .WithMessage(Get<Strings>(x => x.SpecifyValue));

        RuleFor(x => x.BrowserParameters)
            .NotEmpty()
            .WithMessage(Get<Strings>(x => x.SpecifyBrowserParameters));
        
        RuleFor(x => x.ChallengeWindowSize)
            .NotEmpty()
            .WithMessage(Get<Strings>(x => x.SpecifyChallengeWindowSize));
        
        RuleFor(x => x.ReturnUrl)
            .NotEmpty()
            .WithMessage(Get<Strings>(x => x.SpecifyReturnUrl));
        
        RuleFor(x => x.ApplePayToken.ApplicationData)
            .NotEmpty()
            .When(x => x.ApplePayToken.HasValue())
            .WithMessage(Get<Strings>(x => x.SpecifyApplicationData));
        
        RuleFor(x => x.ApplePayToken.DisplayName)
            .NotEmpty()
            .When(x => x.ApplePayToken.HasValue())
            .WithMessage(Get<Strings>(x => x.SpecifyDisplayName));
        
        RuleFor(x => x.ApplePayToken.PaymentData)
            .NotEmpty()
            .When(x => x.ApplePayToken.HasValue())
            .WithMessage(Get<Strings>(x => x.SpecifyPaymentData));

        RuleFor(x => x.ApplePayToken.SessionValidationToken)
            .NotEmpty()
            .When(x => x.ApplePayToken.HasValue())
            .WithMessage(Get<Strings>(x => x.SpecifySessionValidationToken)); }

    public class Strings : ValidationStrings {
        public string SinglePaymentSourceRequired => "A card identifier or GooglePay token must be specified";
        public string SpecifyBrowserParameters => "Please specify the browser parameters";
        public string SpecifyChallengeWindowSize => "Please specify the challenge window size";
        public string SpecifyMerchantSessionKey => "Please specify the merchant session key";
        public string SpecifyReturnUrl => "Please specify the return URL";
        public string SpecifyApplicationData => "The Application Data must be specified in the Apple Pay Token";
        public string SpecifyDisplayName => "The Display Name must be specified in the Apple Pay Token";
        public string SpecifyPaymentData => "The Payment Data must be specified in the Apple Pay Token";
        public string SpecifySessionValidationToken => "The Session Validation Token must be specified in the Apple Pay Token";
        
        public string SpecifyValue => "Please specify the value";
    }
}

