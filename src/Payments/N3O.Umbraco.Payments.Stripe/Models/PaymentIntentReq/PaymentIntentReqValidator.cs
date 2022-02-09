using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public class PaymentIntentReqValidator : ModelValidator<PaymentIntentReq> {
        public PaymentIntentReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.PaymentMethodId)
                .NotEmpty()
                .WithMessage(Get<Strings>(s => s.SpecifyPaymentMethodId));
            
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage(Get<Strings>(s => s.SpecifyCustomerId));
            
            // TODO Add validation for (configurable) minimum payment value
            RuleFor(x => x.Value)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyValue));
        }

        public class Strings : ValidationStrings {
            public string SpecifyCustomerId => "Please specify the customer ID";
            public string SpecifyPaymentMethodId => "Please specify the payment method ID";
            public string SpecifyValue => "Please specify the payment value";
        }
    }
}