using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Opayo.Models;

public class ChargeCardReqValidator : ModelValidator<ChargeCardReq> {
    public ChargeCardReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x)
            .Must(HaveExactlyOneSource)
            .WithMessage(Get<Strings>(x => x.ProvideOneAuthMethod));
        When(x => string.IsNullOrWhiteSpace(x.GooglePayToken), () => {
            RuleFor(x => x.MerchantSessionKey)
                .NotEmpty()
                .WithMessage(Get<Strings>(x => x.SpecifyMerchantSessionKey));

            RuleFor(x => x.CardIdentifier)
                .NotEmpty()
                .WithMessage(Get<Strings>(x => x.SpecifyCardIdentifier));
        });
        
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

    private bool HaveExactlyOneSource(ChargeCardReq req) {
        bool hasCard = !string.IsNullOrWhiteSpace(req.CardIdentifier) && !string.IsNullOrWhiteSpace(req.MerchantSessionKey);
        bool hasToken = !string.IsNullOrWhiteSpace(req.GooglePayToken);

        return (hasCard ^ hasToken);
    }

    public class Strings : ValidationStrings {
        public string SpecifyBrowserParameters => "Please specify the browser parameters";
        public string SpecifyChallengeWindowSize => "Please specify the challenge window size";
        public string SpecifyCardIdentifier => "Please specify the card identifier";
        public string SpecifyMerchantSessionKey => "Please specify the merchant session key";
        public string SpecifyReturnUrl => "Please specify the return URL";
        public string SpecifyValue => "Please specify the value";
        public string ProvideOneAuthMethod => "Please provide either a card identifier with merchant session key, or a Google Pay token — not both.";
    }
}

