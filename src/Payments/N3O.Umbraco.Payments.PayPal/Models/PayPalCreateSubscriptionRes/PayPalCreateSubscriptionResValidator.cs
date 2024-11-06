using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.PayPal.Models.PayPalCreatePlanSubscriptionReq;

public class PayPalCreateSubscriptionResValidator : ModelValidator<PayPalCreateSubscriptionReq> {
    public PayPalCreateSubscriptionResValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.ReturnUrl)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyReturnUrl));
        
        RuleFor(x => x.CancelUrl)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyCancelUrl));
    }

    public class Strings : ValidationStrings {
        public string SpecifyReturnUrl => "Please specify the return URL";
        public string SpecifyCancelUrl => "Please specify the cancel URL";
    }
}