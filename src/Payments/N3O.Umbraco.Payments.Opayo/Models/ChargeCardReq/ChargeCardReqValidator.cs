using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Opayo.Models;

public class ChargeCardReqValidator : ModelValidator<ChargeCardReq> {
    public ChargeCardReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x)
            .Must(x => x.CardIdentifier.HasValue() || x.GooglePayToken.HasValue())
            .WithMessage(Get<Strings>(x => x.SinglePaymentSourceRequired));
        
        RuleFor(x => x)
            .Must(x => !x.CardIdentifier.HasValue() || !x.GooglePayToken.HasValue())
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
    }

    public class Strings : ValidationStrings {
        public string SinglePaymentSourceRequired => "A card identifier or GooglePay token must be specified";
        public string SpecifyBrowserParameters => "Please specify the browser parameters";
        public string SpecifyChallengeWindowSize => "Please specify the challenge window size";
        public string SpecifyMerchantSessionKey => "Please specify the merchant session key";
        public string SpecifyReturnUrl => "Please specify the return URL";
        public string SpecifyValue => "Please specify the value";
    }
}

