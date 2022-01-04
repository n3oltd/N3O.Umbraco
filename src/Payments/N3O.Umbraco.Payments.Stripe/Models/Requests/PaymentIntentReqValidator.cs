using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public class PaymentIntentReqValidator : ModelValidator<PaymentIntentReq> {
        public PaymentIntentReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.Value)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyValue));
        }

        public class Strings : ValidationStrings {
            public string SpecifyValue => "Please specify the payment value";
        }
    }
}