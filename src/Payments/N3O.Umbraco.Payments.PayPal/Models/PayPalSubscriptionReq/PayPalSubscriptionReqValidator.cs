using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.PayPal.Models;

public class PayPalSubscriptionReqValidator : ModelValidator<PayPalSubscriptionReq> {
    public PayPalSubscriptionReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.SubscriptionId)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifySubscriptionId));
    }
    
    public class Strings : ValidationStrings {
        public string SpecifySubscriptionId => "Please specify the subscription ID.";
    }
}