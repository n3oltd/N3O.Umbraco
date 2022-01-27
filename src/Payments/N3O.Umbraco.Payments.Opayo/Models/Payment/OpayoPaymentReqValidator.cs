using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public class OpayoPaymentReqValidator : ModelValidator<OpayoPaymentReq> {
        public OpayoPaymentReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.CallbackUrl)
               .NotEmpty()
               .WithMessage(Get<Strings>(x => x.SpecifyCallbackUrl));
            
            RuleFor(x => x.Value)
               .NotEmpty()
               .WithMessage(Get<Strings>(x => x.SpecifyValue));
            
            RuleFor(x => x.MerchantSessionKey)
               .NotEmpty()
               .WithMessage(Get<Strings>(x => x.SpecifyMerchantSessionKey));

            RuleFor(x => x.CardIdentifier)
               .NotEmpty()
               .WithMessage(Get<Strings>(x => x.SpecifyCardIdentifier));
            
            RuleFor(x => x.BrowserParameters)
               .NotEmpty()
               .WithMessage(Get<Strings>(x => x.SpecifyBrowserParameters));
        }

        public class Strings : ValidationStrings {
            public string SpecifyBrowserParameters => "Please specify the specify browser parameters";
            public string SpecifyCallbackUrl => "Please specify the callback url for strong customer authentication";
            public string SpecifyCardIdentifier => "Please specify the card identifier";
            public string SpecifyMerchantSessionKey => "Please specify the merchant session key";
            public string SpecifyValue => "Please specify the value";
        }
    }
}