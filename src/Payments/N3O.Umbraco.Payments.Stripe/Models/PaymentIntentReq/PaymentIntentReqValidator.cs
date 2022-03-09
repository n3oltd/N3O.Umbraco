using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public class PaymentIntentReqValidator : ModelValidator<PaymentIntentReq> {
        public PaymentIntentReqValidator(IFormatter formatter) : base(formatter) {
            // TODO Add validation for (configurable) minimum payment value
            RuleFor(x => x.Value)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyValue));
            
            RuleFor(x => x.PaymentMethodId)
               .NotNull()
               .WithMessage(Get<Strings>(s => s.PaymentMethodId));
        }

        public class Strings : ValidationStrings {
            public string SpecifyValue => "Please specify the payment value";
            public string PaymentMethodId => "Please specify the payment method ID";
        }
    }
}