using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.Stripe.Models;

public class SetupIntentReqValidator : ModelValidator<SetupIntentReq> {
    public SetupIntentReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.PaymentMethodId)
           .NotEmpty()
           .WithMessage(Get<Strings>(s => s.SpecifyPaymentMethodId));
    }

    public class Strings : ValidationStrings {
        public string SpecifyPaymentMethodId => "Please specify the payment method ID";
    }
}
