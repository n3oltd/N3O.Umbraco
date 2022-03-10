using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public class SetupIntentReqValidator : ModelValidator<SetupIntentReq> {
        public SetupIntentReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.PaymentMethodId)
               .NotNull()
               .WithMessage(Get<Strings>(s => s.PaymentMethodId));
        }

        public class Strings : ValidationStrings {
            public string PaymentMethodId => "Please specify the payment method ID";
        }
    }
}