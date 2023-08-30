using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public class PaymentProcessedReqValidator : ModelValidator<PaymentProcessedReq> {
    public PaymentProcessedReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage(Get<Strings>(x => x.SpecifyPaymentId));
    }

    public class Strings : ValidationStrings {
        public string SpecifyPaymentId => "Please specify the payment ID";
    }
}
