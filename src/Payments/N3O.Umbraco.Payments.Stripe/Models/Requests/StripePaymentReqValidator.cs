using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public class StripePaymentReqValidator : ModelValidator<StripePaymentReq> {
        public StripePaymentReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.PaymentIntentId)
                .NotEmpty()
                .WithMessage(Get<Strings>(s => s.SpecifyPaymentIntentId));

            RuleFor(x => x.PaymentMethodId)
                .NotEmpty()
                .WithMessage(Get<Strings>(s => s.SpecifyPaymentMethodId));
        }
    }
    
    public class Strings : ValidationStrings {
        public string SpecifyPaymentMethodId => "Please specify the payment method ID";
        public string SpecifyPaymentIntentId => "Please specify the PaymentIntent ID";
    }
}