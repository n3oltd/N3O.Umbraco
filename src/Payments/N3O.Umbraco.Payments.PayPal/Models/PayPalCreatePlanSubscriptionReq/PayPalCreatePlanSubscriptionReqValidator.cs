using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.PayPal.Models.PayPalCreatePlanSubscriptionReq;

public class PayPalCreatePlanSubscriptionReqValidator : ModelValidator<PayPalCreatePlanSubscriptionReq> {
    public PayPalCreatePlanSubscriptionReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.PlanName)
           .NotNull()
           .WithMessage(Get<Strings>(s => s.SpecifyPlanName));
    }

    public class Strings : ValidationStrings {
        public string SpecifyPlanName => "Please specify a plan name";
    }
}