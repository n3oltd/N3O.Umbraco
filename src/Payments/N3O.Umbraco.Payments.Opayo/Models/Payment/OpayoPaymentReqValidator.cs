using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public class OpayoPaymentReqValidator : ModelValidator<OpayoPaymentReq> {
        public OpayoPaymentReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.Value)
               .NotEmpty()
               .WithMessage(Get<Strings>(x => x.SpecifyValue));
            
            RuleFor(x => x.MerchantSessionKey)
               .NotEmpty()
               .WithMessage(Get<Strings>(x => x.SpecifyMerchantSessionKey));

            RuleFor(x => x.CardIdentifier)
               .NotEmpty()
               .WithMessage(Get<Strings>(x => x.SpecifyCardIdentifier));
        }

        public class Strings : ValidationStrings {
            public string SpecifyCardIdentifier => "Please specify the card identifier";
            public string SpecifyMerchantSessionKey => "Please specify the merchant session key";
            public string SpecifyValue => "Please specify the value";
        }
    }
}